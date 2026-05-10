using Microsoft.AspNetCore.Mvc;
using VideoKyc.Application.Interfaces;
using VideoKyc.Application.Models;

namespace VideoKyc.API.Controllers
{
    [ApiController]
    [Route("api/kyc")]
    public class KycController : ControllerBase
    {
        private readonly IKycService _kycService;

        public KycController(IKycService kycService)
        {
            _kycService = kycService;
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save(KycDetailRequest request)
        {
            await _kycService.SaveKycDetails(request);

            return Ok();
        }
    }
}