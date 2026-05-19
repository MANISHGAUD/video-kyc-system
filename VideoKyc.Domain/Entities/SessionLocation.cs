using System.Text.Json.Serialization;

namespace VideoKyc.Domain.Entities
{
    public class SessionLocation
    {
        public Guid Id { get; set; }

        public Guid SessionId { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double? Accuracy { get; set; }

        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public UserSession? Session { get; set; }
    }
}