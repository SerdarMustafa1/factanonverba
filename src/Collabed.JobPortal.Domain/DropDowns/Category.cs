using Volo.Abp.Domain.Entities;

namespace Collabed.JobPortal.DropDowns
{
    public class Category : AggregateRoot<int>
    {
        public string Name { get; set; }

        /* This constructor is for deserialization / ORM purpose */
        private Category()
        {
        }

        public Category(int id, string name)
        {
            Name = name;
        }
    }
}
