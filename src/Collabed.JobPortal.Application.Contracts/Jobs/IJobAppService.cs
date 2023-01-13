using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Collabed.JobPortal.Jobs
{
    public interface IJobAppService
    {
        Task<JobDto> GetAsync(Guid id);
        Task<PagedResultDto<JobDto>> GetListAsync(JobGetListInput input);
        Task<JobDto> CreateAsync(CreateJobDto input);
        Task UpdateAsync(Guid id, CreateUpdateJobDto input);
        Task DeleteAsync(Guid id);
    }
}
