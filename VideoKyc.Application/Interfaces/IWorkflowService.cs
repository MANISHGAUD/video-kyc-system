using VideoKyc.Domain;

namespace VideoKyc.Application.Interfaces
{
    public interface IWorkflowService
    {
        Task<bool> ChangeState(
            Guid sessionId,

            SessionStatus newStatus,

            string changedBy,

            string? remarks = null);
    }
}