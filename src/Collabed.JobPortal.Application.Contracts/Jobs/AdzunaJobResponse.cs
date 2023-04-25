using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Collabed.JobPortal.Jobs
{
    public class AdzunaJobResponse
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }
        [JsonPropertyName("results")]
        public IEnumerable<AdzunaJobResult> Results { get; set; }
    }

    public class AdzunaJobResult
    {
        [JsonPropertyName("id")]
        public string Reference { get; set; }
        [JsonPropertyName("location")]
        public ExtJobLocation JobLocation { get; set; }
        [JsonPropertyName("company")]
        public Company Company { get; set; }
        [JsonPropertyName("redirect_url")]
        public string ApplicationUrl { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("salary_min")]
        public float? SalaryFrom { get; set; }
        [JsonPropertyName("salary_max")]
        public float? SalaryTo { get; set; }
        [JsonPropertyName("contract_type")]
        public string ContractType { get; set; }
        [JsonPropertyName("contract_time")]
        public string ContractTime { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("salary_is_predicted")]
        public string IsSalaryEstimated { get; set; }
        [JsonPropertyName("created")]
        public string PostDate { get; set; }
    }

    public class ExtJobLocation
    {
        [JsonPropertyName("display_name")]
        public string Name { get; set; }
    }
    public class Company
    {
        [JsonPropertyName("display_name")]
        public string Name { get; set; }
    }

    public class AdzunaJobRequest
    {

    }
}
