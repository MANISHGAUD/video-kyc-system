namespace VideoKyc.Domain.Entities
{
    public class ChatMessage
    {
        public Guid Id { get; set; }

        public Guid SessionId { get; set; }

        public string SenderType { get; set; }

        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}