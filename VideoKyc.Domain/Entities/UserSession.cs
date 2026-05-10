using VideoKyc.Domain;

public class UserSession
{
    public Guid Id { get; set; }

    public string UserId { get; set; }

    public string UserConnectionId { get; set; }

    public string? AdminConnectionId { get; set; }

    public SessionStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? EndedAt { get; set; }
}