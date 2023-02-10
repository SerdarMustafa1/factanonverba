using System;
using Volo.Abp.Application.Dtos;

namespace Collabed.JobPortal.Organisations
{
    public class OrganisationDto : ExtensibleEntityDto<Guid>
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
    }
}
