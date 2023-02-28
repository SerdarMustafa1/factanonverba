using System.Text.Json.Serialization;

namespace Collabed.JobPortal.Jobs
{
	public class JobResponse
	{
		[JsonPropertyName("status")]
		public string Status { get; }
		[JsonPropertyName("job_url")]
		public string JobUrl { get; }
		[JsonPropertyName("message")]
		public string Message { get; }

		public JobResponse(string status, string jobUrl, string message)
		{
			Status = status;
			JobUrl = jobUrl;
			Message = message;
		}
	}
}
