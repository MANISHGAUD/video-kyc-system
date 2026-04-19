using System;
using System.Collections.Generic;
using System.Text;
using VideoKyc.Domain.Entities;

namespace VideoKyc.Application.Interfaces
{
    public interface ISessionService
    {
        Task<UserSession> CreateSession(string userId, string connectionId);
        Task<List<UserSession>> GetWaitingUsers();
        Task<UserSession?> TryAssignAdmin(string userConnectionId, string adminConnectionId);
        Task EndSession(string connectionId);
    }
}
