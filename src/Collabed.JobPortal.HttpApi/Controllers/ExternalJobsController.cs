using Collabed.JobPortal.Jobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace Collabed.JobPortal.Controllers
{
	public class ExternalJobsController : AbpController
	{
		private readonly ILogger<ExternalJobsController> _logger;
		private readonly IJobAppService _jobAppService;

		public ExternalJobsController(ILogger<ExternalJobsController> logger, IJobAppService jobAppService)
		{
			_logger = logger;
			_jobAppService = jobAppService;
		}

		[HttpPost]
		[Route("externalJobFeed")]
		public async Task<IActionResult> FeedExternalJobsAsync([FromBody] ExternalJobRequest jobRequest)
		{
			_logger.LogInformation($"Received external job feed. Request: {jobRequest}");

			var result = await _jobAppService.HandleExternalJobFeedAsync(jobRequest);
			var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

			var response = new JobResponse(result.Status, result.Status  == JobResponseCodes.Success ? $"{baseUrl}/Job?jobReference={result.JobReference}" : "", result.Message);
			return Ok(response);
		}
	}
}
