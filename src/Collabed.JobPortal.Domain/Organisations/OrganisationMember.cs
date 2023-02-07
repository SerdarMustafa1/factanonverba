using System;
using Volo.Abp.Domain.Entities;

namespace Collabed.JobPortal.Organisations
{
    public class OrganisationMember : Entity
    {
        public Guid OrganisationId { get; private set; }
        public Guid UserId { get; private set; }

        /* This constructor is for deserialization / ORM purpose */
        private OrganisationMember()
        {
        }

        internal OrganisationMember(Guid organisationId, Guid userId)
        {
            OrganisationId = organisationId;
            UserId = userId;
        }

        public override object[] GetKeys()
        {
            return new object[] { OrganisationId, UserId };
        }
    }
}
