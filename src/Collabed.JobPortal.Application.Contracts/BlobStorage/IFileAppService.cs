using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Collabed.JobPortal.BlobStorage
{
    public interface IFileAppService : IApplicationService
    {
        Task SaveBlobAsync(SaveBlobInputDto input);
        Task<BlobDto> GetBlobAsync(GetBlobRequestDto input);
        Task<bool> DeleteBlobAsync(string blobName);
    }
}
