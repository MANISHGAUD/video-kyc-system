namespace VideoKyc.Domain.Entities
{
    public class SessionStateHistory
    {
        public Guid Id { get; set; }

        public Guid SessionId { get; set; }

        public string OldStatus { get; set; }

        public string NewStatus { get; set; }

        public string? ChangedBy { get; set; }

        public string? Remarks { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}