using VideoKyc.Domain.Entities;

namespace VideoKyc.Application.Interfaces
{
    public interface IChatService
    {
        Task SaveMessage(Guid sessionId,string senderType,string message);
    }
}