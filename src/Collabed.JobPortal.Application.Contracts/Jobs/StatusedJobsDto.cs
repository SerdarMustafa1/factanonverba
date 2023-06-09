namespace Collabed.JobPortal.Jobs
{
    public class StatusedJobsDto
    {
        public int AllJobsCount { get; set; }
        public int LiveCount { get; set; }
        public int HiringCount { get; set; }
        public int ClosedCount { get; set; }
    }
}
