using System;
using Volo.Abp.Application.Dtos;

namespace Collabed.JobPortal.Jobs
{
    public class JobGetListInput : PagedAndSortedResultRequestDto
    {
        public Guid? OrganisationId { get; set; }
    }
}
