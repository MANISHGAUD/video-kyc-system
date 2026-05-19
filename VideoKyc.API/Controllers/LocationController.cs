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

        private readonly IHubContext<CallHub> _hub;

        public LocationController(AppDbContext context, IHubContext<CallHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] SessionLocation request)
        {
            request.Id = Guid.NewGuid();

            request.CreatedAt = DateTime.UtcNow;

            _context.SessionLocations.Add(request);

            await _context.SaveChangesAsync();

            var waitingUsers =
                await _context.UserSessions.Include(x => x.Location).
                Where(x => x.Status == SessionStatus.Waiting).OrderBy(x => x.CreatedAt).ToListAsync();

            await _hub.Clients.Group("Admins").SendAsync("UpdateQueue", waitingUsers);

            return Ok();
        }
    }
}