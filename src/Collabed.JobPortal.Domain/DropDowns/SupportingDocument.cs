using Volo.Abp.Domain.Entities;

namespace Collabed.JobPortal.DropDowns
{
    public class SupportingDocument : AggregateRoot<int>
    {
        public string Name { get; set; }

        /* This constructor is for deserialization / ORM purpose */
        private SupportingDocument()
        {
        }

        public SupportingDocument(int id, string name)
        {
            Name = name;
        }
    }
}
