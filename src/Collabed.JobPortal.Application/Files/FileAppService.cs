﻿using Collabed.JobPortal.BlobStorage;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;

namespace Collabed.JobPortal.Files
{
    public class FileAppService : ApplicationService, IFileAppService
    {
        private readonly IBlobContainer<JobPortalContainer> _fileContainer;

        public FileAppService(IBlobContainer<JobPortalContainer> fileContainer)
        {
            _fileContainer = fileContainer;
        }

        public async Task SaveBlobAsync(SaveBlobInputDto input)
        {
            await _fileContainer.SaveAsync(input.Name, input.Content, true);
        }

        public async Task<BlobDto> GetBlobAsync(GetBlobRequestDto input)
        {
            var blob = await _fileContainer.GetAllBytesAsync(input.Name);

            return new BlobDto
            {
                Name = input.Name,
                Content = blob
            };
        }

        public async Task<bool> DeleteBlobAsync(string blobName)
        {
            return await _fileContainer.DeleteAsync(blobName);
        }
    }
}
