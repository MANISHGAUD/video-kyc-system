namespace VideoKyc.Domain.Entities
{
    public class SessionDeviceInfo
    {
        public Guid Id { get; set; }

        public Guid SessionId { get; set; }

        public string? UserAgent { get; set; }

        public string? Platform { get; set; }

        public int? ScreenWidth { get; set; }

        public int? ScreenHeight { get; set; }

        public string? Language { get; set; }

        public bool CameraAvailable { get; set; }

        public bool MicrophoneAvailable { get; set; }

        public bool GpsAvailable { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}