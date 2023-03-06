using Volo.Abp.Domain.Entities;

namespace Collabed.JobPortal.DropDowns
{
    public class Location : AggregateRoot<int>
    {
        public string Name { get; set; }
        public decimal Longitude { get; private set; }
        public decimal Latitude { get; private set; }
        public string Country { get; set; }

        /* This constructor is for deserialization / ORM purpose */
        private Location()
        {
        }

        public Location(int id, string name)
        {
            Name = name;
        }

        public void SetLongitude(decimal longitude)
        {
            Longitude = longitude;
        }

        public void SetLatitude(decimal latitude)
        {
            Latitude = latitude;
        }
    }
}
