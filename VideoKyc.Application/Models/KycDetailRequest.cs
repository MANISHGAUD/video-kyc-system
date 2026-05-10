namespace VideoKyc.Application.Models
{
    public class KycDetailRequest
    {
        public Guid SessionId { get; set; }

        public string? PanNumber { get; set; }

        public string? FullName { get; set; }

        public string? Dob { get; set; }

        public string? Address { get; set; }

        public string? Occupation { get; set; }

        public bool ConsentGiven { get; set; }

        public string? Remarks { get; set; }
    }
}