namespace Collabed.JobPortal.Jobs
{
	public class JobResponseDto
	{
		public string Status { get; }
		public string JobReference { get; }
		public string Message { get; }

		public JobResponseDto(string status, string jobReference, string message)
		{
			Status = status;
			JobReference = jobReference;
			Message = message;
		}
	}
}
