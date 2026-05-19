using Microsoft.EntityFrameworkCore;
using VideoKyc.Application.Interfaces;
using VideoKyc.Domain;
using VideoKyc.Infrastructure.Data;

namespace VideoKyc.Infrastructure.Services
{
    public class SessionService : ISessionService
    {
        private readonly AppDbContext _context;

        public SessionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserSession> CreateSession(string userId,string connectionId)
        {
            var session = new UserSession
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                UserConnectionId = connectionId,
                Status = SessionStatus.Waiting,
                CreatedAt = DateTime.UtcNow
            };

            _context.UserSessions.Add(session);

            await _context.SaveChangesAsync();

            return session;
        }

        public async Task<List<UserSession>> GetWaitingUsers()
        {
            return await _context.UserSessions.Include(x => x.Location).
                Where(x => x.Status == SessionStatus.Waiting).OrderBy(x => x.CreatedAt).ToListAsync();
        }

        public async Task<UserSession?> TryAssignAdmin(string userConnectionId,string adminConnectionId)
        {
            var session = await _context.UserSessions
                .FirstOrDefaultAsync(x =>
                    x.UserConnectionId == userConnectionId);

            if (session == null)
                return null;

            if (session.Status != SessionStatus.Waiting)
                return null;

            session.Status = SessionStatus.Connecting;
            session.AdminConnectionId = adminConnectionId;

            await _context.SaveChangesAsync();

            return session;
        }

        public async Task EndSession(string connectionId)
        {
            var session = await _context.UserSessions
                .FirstOrDefaultAsync(x =>
                    x.UserConnectionId == connectionId ||
                    x.AdminConnectionId == connectionId);

            if (session == null)
                return;

            session.Status = SessionStatus.Ended;
            session.EndedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}