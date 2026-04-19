using System;
using System.Collections.Generic;
using System.Text;

namespace VideoKyc.Domain.Entities
{
    public class UserSession
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserConnectionId { get; set; }

        public string? AdminConnectionId { get; set; }

        public SessionStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
