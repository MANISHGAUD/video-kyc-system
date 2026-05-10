using VideoKyc.Application.Interfaces;
using VideoKyc.Application.Models;
using VideoKyc.Domain.Entities;
using VideoKyc.Infrastructure.Data;

namespace VideoKyc.Infrastructure.Services
{
    public class KycService : IKycService
    {
        private readonly AppDbContext _context;

        public KycService(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveKycDetails(KycDetailRequest request)
        {
            var entity = new SessionKycDetail
            {
                Id = Guid.NewGuid(),

                SessionId = request.SessionId,

                PanNumber = request.PanNumber,

                FullName = request.FullName,

                Dob = request.Dob,

                Address = request.Address,

                Occupation = request.Occupation,

                ConsentGiven = request.ConsentGiven,

                Remarks = request.Remarks,

                CreatedAt = DateTime.UtcNow
            };

            _context.SessionKycDetails.Add(entity);

            await _context.SaveChangesAsync();
        }
    }
}