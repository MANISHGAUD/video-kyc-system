using Microsoft.AspNetCore.Mvc;
using VideoKyc.Domain.Entities;
using VideoKyc.Infrastructure.Data;

namespace VideoKyc.API.Controllers
{
    [ApiController]
    [Route("api/recording")]
    public class RecordingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecordingController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] Guid sessionId)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "recordings");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName = Guid.NewGuid() + ".webm";

            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var entity = new SessionRecording
            {
                Id = Guid.NewGuid(),

                SessionId = sessionId,

                FileName = fileName,

                FilePath =  "/recordings/" + fileName,

                CreatedAt = DateTime.UtcNow
            };

            _context.SessionRecordings.Add(entity);

            await _context.SaveChangesAsync();

            return Ok(entity);
        }
    }
}