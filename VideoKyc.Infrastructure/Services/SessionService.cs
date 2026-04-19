using System;
using System.Collections.Generic;
using System.Text;
using VideoKyc.Application.Interfaces;
using VideoKyc.Domain;
using VideoKyc.Domain.Entities;

namespace VideoKyc.Infrastructure.Services
{
    public class SessionService : ISessionService
    {
        private static List<UserSession> _sessions = new();
        private static readonly object _lock = new();

        public Task<UserSession> CreateSession(string userId, string connectionId)
        {
            var session = new UserSession
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                UserConnectionId = connectionId,
                Status = SessionStatus.Waiting,
                CreatedAt = DateTime.UtcNow
            };

            lock (_lock)
            {
                _sessions.Add(session);
            }

            return Task.FromResult(session);
        }

        public Task<List<UserSession>> GetWaitingUsers()
        {
            lock (_lock)
            {
                return Task.FromResult(_sessions
                    .Where(x => x.Status == SessionStatus.Waiting)
                    .ToList());
            }
        }

        public Task<UserSession?> TryAssignAdmin(string userConnectionId, string adminConnectionId)
        {
            lock (_lock)
            {
                var session = _sessions.FirstOrDefault(x => x.UserConnectionId == userConnectionId);

                if (session == null) return Task.FromResult<UserSession?>(null);

                // ❗ CRITICAL CHECK
                if (session.Status != SessionStatus.Waiting)
                    return Task.FromResult<UserSession?>(null);

                session.Status = SessionStatus.Connecting;
                session.AdminConnectionId = adminConnectionId;

                return Task.FromResult<UserSession?>(session);
            }
        }

        public Task EndSession(string connectionId)
        {
            lock (_lock)
            {
                var session = _sessions.FirstOrDefault(x =>
                    x.UserConnectionId == connectionId ||
                    x.AdminConnectionId == connectionId);

                if (session != null)
                    session.Status = SessionStatus.Ended;
            }

            return Task.CompletedTask;
        }
    }
}
