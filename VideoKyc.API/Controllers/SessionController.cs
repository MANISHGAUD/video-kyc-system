using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoKyc.Infrastructure.Data;

[ApiController]
[Route("api/session")]
public class SessionController : ControllerBase
{
    private readonly AppDbContext _context;

    public SessionController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("details/{sessionId}")]
    public async Task<IActionResult> GetDetails(Guid sessionId)
    {
        var session = await _context.UserSessions.Include(x => x.Location).Include(x => x.DeviceInfo).FirstOrDefaultAsync(x =>  x.Id == sessionId);

        if (session == null)
            return NotFound();

        return Ok(session);
    }
}