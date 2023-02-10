using System;
using Volo.Abp.Application.Dtos;

namespace Collabed.JobPortal.Organisations
{
    public class CreateOrganisationDto : ExtensibleEntityDto
    {
        public Guid OwnerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
