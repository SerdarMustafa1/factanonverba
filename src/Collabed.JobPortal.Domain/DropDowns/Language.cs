using Volo.Abp.Domain.Entities;

namespace Collabed.JobPortal.DropDowns
{
    public class Language : AggregateRoot<int>
    {
        public string Name { get; set; }

        /* This constructor is for deserialization / ORM purpose */
        private Language()
        {
        }

        public Language(int id, string name)
        {
            Name = name;
        }
    }
}
