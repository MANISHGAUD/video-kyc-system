using VideoKyc.Application.Models;

namespace VideoKyc.Application.Interfaces
{
    public interface IKycService
    {
        Task SaveKycDetails(KycDetailRequest request);
    }
}