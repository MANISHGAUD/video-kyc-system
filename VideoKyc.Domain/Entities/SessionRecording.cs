namespace VideoKyc.Domain.Entities
{
    public class SessionRecording
    {
        public Guid Id { get; set; }

        public Guid SessionId { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}