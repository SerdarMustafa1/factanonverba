﻿using Collabed.JobPortal.Types;
using Volo.Abp.Application.Dtos;

namespace Collabed.JobPortal.Jobs
{
    public class SearchCriteriaInput : PagedAndSortedResultRequestDto
    {
        public int CategoryId { get; set; }
        public string Keyword { get; set; }
        public string Location { get; set; }
        public int? SearchRadius { get; set; }
        public bool? NetZero { get; set; }
        public ContractType? ContractType { get; set; }
        public EmploymentType? EmploymentType { get; set; }
        public JobLocation? Workplace { get; set; }
        public int? SalaryMinimum { get; set; }
        public int? SalaryMaximum { get; set; }
    }
}