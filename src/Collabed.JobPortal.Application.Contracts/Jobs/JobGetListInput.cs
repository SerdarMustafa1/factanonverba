using Collabed.JobPortal.Types;
using System;
using Volo.Abp.Application.Dtos;

namespace Collabed.JobPortal.Jobs
{
    public class JobGetListInput : PagedAndSortedResultRequestDto
    {
        public Guid? OrganisationId { get; set; }
        public string SearchCriteria { get; set; }
        public JobStatus? Status { get; set; }
    }
}
