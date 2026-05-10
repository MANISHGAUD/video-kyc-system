using Microsoft.AspNetCore.Mvc;

namespace VideoKyc.API.Controllers
{
    [ApiController]
    [Route("api/turn")]
    public class TurnController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetTurnCredentials()
        {
            return Ok(new
            {
                iceServers = new[]
                {
                    new
                    {
                        urls = new[]
                        {
                            "stun:stun.relay.metered.ca:80",
                            "turn:global.relay.metered.ca:80",
                            "turn:global.relay.metered.ca:80?transport=tcp",
                            "turn:global.relay.metered.ca:443",
                            "turns:global.relay.metered.ca:443?transport=tcp"
                        },
                        username = "ec89d1d30e157d8b291c172c",
                        credential = "QEn/hIYXcJ5souCu"
                    }
                }
            });
        }
    }
}