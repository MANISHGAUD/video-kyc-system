
using Microsoft.AspNetCore.SignalR;
using VideoKyc.Application.Interfaces;
namespace VideoKyc.API.Hubs
{

    public class CallHub : Hub
    {
        private readonly ISessionService _sessionService;

        private readonly IChatService _chatService;

        public CallHub(ISessionService sessionService,IChatService chatService)
        {
            _sessionService = sessionService;
            _chatService = chatService;
        }

        public async Task<Guid> JoinQueue(string userId)
        {
            var session = await _sessionService.CreateSession(userId,Context.ConnectionId);

            var waiting = await _sessionService.GetWaitingUsers();

            await Clients.Group("Admins").SendAsync("UpdateQueue", waiting);

            return session.Id;
        }

        public async Task JoinAdmin()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");

            var waiting = await _sessionService.GetWaitingUsers();
            await Clients.Caller.SendAsync("UpdateQueue", waiting);
        }

        public async Task StartCall(string userConnectionId)
        {
            var session = await _sessionService.TryAssignAdmin(userConnectionId,Context.ConnectionId);

            if (session == null)
            {
                await Clients.Caller.SendAsync("UserAlreadyTaken");
                return;
            }

            await Clients.Client(userConnectionId).SendAsync("CallStarted", Context.ConnectionId);

            var waiting = await _sessionService.GetWaitingUsers();

            await Clients.Group("Admins").SendAsync("UpdateQueue", waiting);
        }

        public async Task SendOffer(string targetId, string offer)
        {
            await Clients.Client(targetId).SendAsync("ReceiveOffer", offer, Context.ConnectionId);
        }

        public async Task SendAnswer(string targetId, string answer)
        {
            await Clients.Client(targetId).SendAsync("ReceiveAnswer", answer);
        }

        public async Task SendIceCandidate(string targetId, string candidate)
        {
            await Clients.Client(targetId).SendAsync("ReceiveIceCandidate", candidate);
        }
        public async Task EndCall()
        {
            await _sessionService.EndSession(Context.ConnectionId);

            await Clients.Others.SendAsync("CallEnded");

            var waiting = await _sessionService.GetWaitingUsers();
            await Clients.Group("Admins").SendAsync("UpdateQueue", waiting);
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await _sessionService.EndSession(Context.ConnectionId);

            var waiting = await _sessionService.GetWaitingUsers();
            await Clients.Group("Admins").SendAsync("UpdateQueue", waiting);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendChatMessage(Guid sessionId,string targetConnectionId,string senderType,string message)
        {
            await _chatService.SaveMessage(sessionId,senderType,message);

            await Clients.Client(targetConnectionId).SendAsync("ReceiveChatMessage",senderType,message);
        }
    }
}
