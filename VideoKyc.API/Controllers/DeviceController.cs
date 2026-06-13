using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoKyc.API.Models;
using VideoKyc.Domain.Entities;
using VideoKyc.Infrastructure.Data;
namespace VideoKyc.API.Controllers
{
    [ApiController]
    [Route("api/device")]
    public class DeviceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DeviceController(
            AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save(
            [FromBody]
            SaveDeviceInfoRequest request)
        {
            var existing = await _context.SessionDeviceInfos.FirstOrDefaultAsync(x => x.SessionId == request.SessionId);

            if (existing != null)
            {
                existing.UserAgent = request.UserAgent;
                existing.Platform = request.Platform;
                existing.ScreenWidth = request.ScreenWidth;
                existing.ScreenHeight = request.ScreenHeight;
                existing.Language = request.Language;
                existing.CameraAvailable = request.CameraAvailable;
                existing.MicrophoneAvailable = request.MicrophoneAvailable;
                existing.GpsAvailable = request.GpsAvailable;
            }
            else
            {
                var device = new SessionDeviceInfo
                {
                    Id = Guid.NewGuid(),
                    SessionId = request.SessionId,
                    UserAgent = request.UserAgent,
                    Platform = request.Platform,
                    ScreenWidth = request.ScreenWidth,
                    ScreenHeight = request.ScreenHeight,
                    Language = request.Language,
                    CameraAvailable = request.CameraAvailable,
                    MicrophoneAvailable = request.MicrophoneAvailable,
                    GpsAvailable = request.GpsAvailable,
                    CreatedAt = DateTime.UtcNow
                };

                _context.SessionDeviceInfos.Add(device);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true
            });
        }
    }
}