
using Microsoft.AspNetCore.SignalR;
using VideoKyc.Application.Interfaces;
namespace VideoKyc.API.Hubs
{

    public class CallHub : Hub
    {
        private readonly ISessionService _sessionService;

        public CallHub(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public async Task JoinQueue(string userId)
        {
            var session = await _sessionService.CreateSession(userId, Context.ConnectionId);

            var waiting = await _sessionService.GetWaitingUsers();
            await Clients.Group("Admins").SendAsync("UpdateQueue", waiting);
        }

        public async Task JoinAdmin()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");

            // ✅ send current queue immediately
            var waiting = await _sessionService.GetWaitingUsers();
            await Clients.Caller.SendAsync("UpdateQueue", waiting);
        }

        public async Task StartCall(string userConnectionId)
        {
            var session = await _sessionService.TryAssignAdmin(
                userConnectionId,
                Context.ConnectionId
            );

            if (session == null)
            {
                // ❌ Already taken
                await Clients.Caller.SendAsync("UserAlreadyTaken");
                return;
            }

            // ✅ Notify user
            await Clients.Client(userConnectionId)
                .SendAsync("CallStarted", Context.ConnectionId);

            // 🔄 Update all admins (remove from queue)
            var waiting = await _sessionService.GetWaitingUsers();
            await Clients.Group("Admins").SendAsync("UpdateQueue", waiting);
        }

        public async Task SendOffer(string targetId, string offer)
        {
            await Clients.Client(targetId)
                .SendAsync("ReceiveOffer", offer, Context.ConnectionId);
        }

        public async Task SendAnswer(string targetId, string answer)
        {
            await Clients.Client(targetId)
                .SendAsync("ReceiveAnswer", answer);
        }

        public async Task SendIceCandidate(string targetId, string candidate)
        {
            await Clients.Client(targetId)
                .SendAsync("ReceiveIceCandidate", candidate);
        }
        public async Task EndCall()
        {
            await _sessionService.EndSession(Context.ConnectionId);

            await Clients.Others.SendAsync("CallEnded");

            // Refresh queue
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
    }
}
