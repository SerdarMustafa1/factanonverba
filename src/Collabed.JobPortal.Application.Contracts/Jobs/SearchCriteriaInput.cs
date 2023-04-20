using Collabed.JobPortal.Types;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Collabed.JobPortal.Jobs
{
    public class SearchCriteriaInput : PagedAndSortedResultRequestDto
    {
        public IEnumerable<int> Categories { get; set; }
        public string Keyword { get; set; }
        public string Location { get; set; }
        public int? SearchRadius { get; set; }
        public int? NetZero { get; set; }
        public ContractType? ContractType { get; set; }
        public EmploymentType? EmploymentType { get; set; }
        public JobLocation? Workplace { get; set; }
        public int? SalaryMinimum { get; set; }
        public int? SalaryMaximum { get; set; }
    }
}
