using Volo.Abp.Domain.Entities;

namespace Collabed.JobPortal.DropDowns
{
    public class SupplementalPay : AggregateRoot<int>
    {
        public string Name { get; set; }

        /* This constructor is for deserialization / ORM purpose */
        private SupplementalPay()
        {
        }

        public SupplementalPay(int id, string name)
        {
            Name = name;
        }
    }
}
