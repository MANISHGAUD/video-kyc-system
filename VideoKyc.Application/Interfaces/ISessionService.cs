using System;
using System.Collections.Generic;
using System.Text;

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
