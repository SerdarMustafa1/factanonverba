using System;
using Volo.Abp.Application.Dtos;

namespace Collabed.JobPortal.Organisations
{
    public class OrganisationDto : ExtensibleEntityDto<Guid>
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string LogoBlobName { get; set; }
        public string LogoFileName { get; set; }
        public string LogoContentType { get; set; }
    }
}
