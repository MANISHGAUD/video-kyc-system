using Microsoft.EntityFrameworkCore;

using VideoKyc.Application.Interfaces;
using VideoKyc.Domain;
using VideoKyc.Domain.Entities;
using VideoKyc.Infrastructure.Data;

namespace VideoKyc.Infrastructure.Services
{
    public class WorkflowService
        : IWorkflowService
    {
        private readonly AppDbContext _context;

        public WorkflowService(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ChangeState(
            Guid sessionId,

            SessionStatus newStatus,

            string changedBy,

            string? remarks = null)
        {
            var session =
                await _context.UserSessions
                    .FirstOrDefaultAsync(
                        x => x.Id == sessionId);

            if (session == null)
                return false;

            // 🚨 VALIDATION
            if (!IsValidTransition(
                session.Status,
                newStatus))
            {
                return false;
            }

            var oldStatus =
                session.Status;

            // Update session state
            session.Status = newStatus;

            // Save history
            var history =
                new SessionStateHistory
                {
                    Id = Guid.NewGuid(),

                    SessionId = sessionId,

                    OldStatus =
                        oldStatus.ToString(),

                    NewStatus =
                        newStatus.ToString(),

                    ChangedBy = changedBy,

                    Remarks = remarks,

                    CreatedAt = DateTime.UtcNow
                };

            _context
                .SessionStateHistories
                .Add(history);

            await _context.SaveChangesAsync();

            return true;
        }

        // 🔥 ENTERPRISE RULE ENGINE
        private bool IsValidTransition(
            SessionStatus current,

            SessionStatus next)
        {
            return (current, next) switch
            {
                // Waiting
                (SessionStatus.Waiting,
                    SessionStatus.Assigned)
                        => true,

                // Assigned
                (SessionStatus.Assigned,
                    SessionStatus.Connecting)
                        => true,

                // Connecting
                (SessionStatus.Connecting,
                    SessionStatus.Live)
                        => true,

                // Live
                (SessionStatus.Live,
                    SessionStatus.Recording)
                        => true,

                // Recording
                (SessionStatus.Recording,
                    SessionStatus.DocumentVerification)
                        => true,

                // Document Verification
                (SessionStatus.DocumentVerification,
                    SessionStatus.FaceVerification)
                        => true,

                // Face Verification
                (SessionStatus.FaceVerification,
                    SessionStatus.Review)
                        => true,

                // Review
                (SessionStatus.Review,
                    SessionStatus.Approved)
                        => true,

                (SessionStatus.Review,
                    SessionStatus.Rejected)
                        => true,

                // Final
                (SessionStatus.Approved,
                    SessionStatus.Completed)
                        => true,

                _ => false
            };
        }
    }
}