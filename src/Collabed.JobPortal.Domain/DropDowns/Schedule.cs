using Volo.Abp.Domain.Entities;

namespace Collabed.JobPortal.DropDowns
{
    public class Schedule : AggregateRoot<int>
    {
        public string Name { get; set; }

        /* This constructor is for deserialization / ORM purpose */
        private Schedule()
        {
        }

        public Schedule(int id, string name)
        {
            Name = name;
        }
    }
}
