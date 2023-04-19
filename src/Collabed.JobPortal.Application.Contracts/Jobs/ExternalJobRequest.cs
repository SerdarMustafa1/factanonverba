using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Collabed.JobPortal.Jobs
{
	public class ExternalJobRequest
	{
		[JsonPropertyName("command")]
		public string Command { get; set; }
		[JsonPropertyName("username")]
		public string Username { get; set; }
		[JsonPropertyName("password")]
		public string Password { get; set; }
		[JsonPropertyName("contact_name")]
		public string ContactName { get; set; }
		[JsonPropertyName("contact_email")]
		public string ContactEmail { get; set; }
		[JsonPropertyName("contact_telephone")]
		public string ContactPhone { get; set; }
		[JsonPropertyName("contact_url")]
		public string ContactUrl { get; set; }
		[JsonPropertyName("days_to_advertise")]
		public int DaysToAdvertise { get; set; }
		[JsonPropertyName("application_email")]
		public string ApplicationEmail { get; set; }
		[JsonPropertyName("application_url")]
		public string ApplicationUrl { get; set; }
		[JsonPropertyName("job_reference")]
		public string Reference { get; set; }
		[JsonPropertyName("job_title")]
		public string Title { get; set; }
		[JsonPropertyName("job_type")]
		public string Type { get; set; }
		[JsonPropertyName("job_duration")]
		public string Duration { get; set; }
		[JsonPropertyName("job_startdate")]
		public string StartDate { get; set; }
		[JsonPropertyName("job_skills")]
		public string Skills { get; set; }
		[JsonPropertyName("job_description")]
		public string Description { get; set; }
		[JsonPropertyName("job_location")]
		public string Location { get; set; }
		[JsonPropertyName("job_industry")]
		public string Industry { get; set; }
		[JsonPropertyName("job_category")]
		public int CategoryId { get; set; }
		[JsonPropertyName("salary_currency")]
		public string SalaryCurrency { get; set; }
		[JsonPropertyName("salary_from")]
		public float? SalaryFrom { get; set; }
		[JsonPropertyName("salary_to")]
		public float? SalaryTo { get; set; }
		[JsonPropertyName("salary_per")]
		public string SalaryPeriod { get; set; }
		[JsonPropertyName("salary_benefits")]
		public string SalaryBenefits { get; set; }
		[JsonPropertyName("salary")]
		public string Salary { get; set; }
		[JsonPropertyName("screening_questions")]
		public IEnumerable<ExtScreeningQuestion> ScreeningQuestions { get; set; }
	}

	public class ExtScreeningQuestion
	{
		[JsonPropertyName("question")]
		public string Question { get; set; }
		[JsonPropertyName("correct_answer")]
		public string CorrectAnswer { get; set; }
	}
}
