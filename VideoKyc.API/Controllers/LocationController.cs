using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using VideoKyc.API.Hubs;
using VideoKyc.Domain;
using VideoKyc.Domain.Entities;
using VideoKyc.Infrastructure.Data;

namespace VideoKyc.API.Controllers
{
    [ApiController]
    [Route("api/location")]
    public class LocationController : ControllerBase
    {
        private readonly AppDbContext _context;


        public LocationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] SessionLocation request)
        {
            request.Id = Guid.NewGuid();

            request.CreatedAt = DateTime.UtcNow;

            var existing = await _context.SessionLocations.FirstOrDefaultAsync(x => x.SessionId == request.SessionId);

            if (existing != null)
            {
                existing.Latitude = request.Latitude;
                existing.Longitude = request.Longitude;
                existing.Accuracy = request.Accuracy;
            }
            else
            {
                request.Id = Guid.NewGuid();
                request.CreatedAt = DateTime.UtcNow;

                _context.SessionLocations.Add(request);
            }

            await _context.SaveChangesAsync();
            await _context.SaveChangesAsync();

           

            return Ok();
        }
    }
}