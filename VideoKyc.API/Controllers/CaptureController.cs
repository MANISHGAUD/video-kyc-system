using Microsoft.AspNetCore.Mvc;
using VideoKyc.Domain.Entities;
using VideoKyc.Infrastructure.Data;

namespace VideoKyc.API.Controllers
{
    [ApiController]
    [Route("api/capture")]
    public class CaptureController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CaptureController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file,[FromForm] Guid sessionId,[FromForm] string captureType)
        {
            if (file == null || file.Length == 0)
                return BadRequest();

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "captures");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = Guid.NewGuid() + ".jpg";

            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var entity = new SessionCapture
            {
                Id = Guid.NewGuid(),

                SessionId = sessionId,

                CaptureType = captureType,

                FileName = fileName,

                FilePath = "/captures/" + fileName,

                CreatedAt = DateTime.UtcNow
            };

            _context.SessionCaptures.Add(entity);

            await _context.SaveChangesAsync();

            return Ok(entity);
        }
    }
}