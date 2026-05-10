namespace VideoKyc.Domain.Entities
{
    public class SessionQuestion
    {
        public Guid Id { get; set; }

        public Guid SessionId { get; set; }

        public string Question { get; set; }

        public string? Answer { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}