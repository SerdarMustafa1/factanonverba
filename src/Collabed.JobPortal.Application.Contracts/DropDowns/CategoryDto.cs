namespace Collabed.JobPortal.DropDowns
{
    public class DropDownDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DropDownDto(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
