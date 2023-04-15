using Collabed.JobPortal.DropDowns;
using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Collabed.JobPortal.Web.Pages.JobDashboard
{
    public class SearchModel : PageModel
    {
        private readonly DropDownAppService _dropDownService;
        private readonly IJobAppService _jobAppService;
        public string DistanceRange { get; set; } = "Within 1 mile";
        public int PaginatedCount { get; set; }
        public int TotalCount { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<string> Category { get; set; }

        public List<int> CategoriesSelected { get; set; } = new List<int>();

        [BindProperty(SupportsGet = true)]
        public string Predicate { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Location { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SelectedRadius { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Sorting { get; set; } = "dateAdded";

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 4;

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalCount, PageSize));

        public IEnumerable<SelectListItem> Categories { get; set; }

        public IEnumerable<JobDto> JobOffers { get; set; }

        public IEnumerable<SelectListItem> AvailableLocationRadius { get; }

        public IEnumerable<SelectListItem> AvailableSortingTypes { get; }

        public IEnumerable<SelectListItem> EmploymentTypes { get; set; }
        public IEnumerable<SelectListItem> ContractTypes { get; set; }
        public IEnumerable<SelectListItem> JobLocations { get; set; }
        public IEnumerable<SelectListItem> SalaryRanges { get; set; }
        public IEnumerable<SelectListItem> NetZeros { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? ContractType { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? EmploymentType { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? JobLocation { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SalaryRange { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? NetZero { get; set; }

        public SearchModel(DropDownAppService dropDownAppService, IJobAppService jobAppService)
        {
            _dropDownService = dropDownAppService;
            _jobAppService = jobAppService;
            AvailableLocationRadius = new SelectListItem[] {
                new SelectListItem("1 mile", "1", true),
                new SelectListItem("5 miles", "5", false),
                new SelectListItem("15 miles", "15", false),
                new SelectListItem("20 miles", "20", false),
                new SelectListItem("25 miles", "25", false),
                new SelectListItem("30 miles", "30", false),
                new SelectListItem("35 miles", "35", false),
                new SelectListItem("40 miles", "40", false),
                new SelectListItem("45 miles", "45", false),
                new SelectListItem("50 miles", "50", false),
                new SelectListItem("60 miles", "60", false),
                new SelectListItem("70 miles", "70", false),
                new SelectListItem("80 miles", "80", false),
                new SelectListItem("90 miles", "90", false),
                new SelectListItem("100 miles", "100", false)
            };
            AvailableSortingTypes = new SelectListItem[] {
                new SelectListItem("Date added", "dateAdded", true),
                new SelectListItem("Closing Date", "closingDate", false),
                new SelectListItem("Salary", "salary", false),
                new SelectListItem("Title", "title", false)
            };
        }

        public async Task OnGetAsync()
        {
            EmploymentTypes = _dropDownService.GetEmploymentTypes().Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            ContractTypes = _dropDownService.GetContractTypes().Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            JobLocations = _dropDownService.GetJobLocations().Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            NetZeros = new List<SelectListItem>
            {
                new SelectListItem("Yes","1")
            };
            SalaryRanges = GetSalaryRanges();
            Categories = (await _dropDownService.GetCategoriesAsync()).Select(x => new SelectListItem(x.Name, x.Id.ToString()));

            CategoriesSelected = ConvertStringParameters(Category);

            var searchInput = new SearchCriteriaInput()
            {
                Categories = CategoriesSelected,
                Keyword = string.IsNullOrWhiteSpace(Predicate) ? string.Empty : Predicate,
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1)* PageSize,
                Location = string.IsNullOrWhiteSpace(Location) ? string.Empty : Location,
                Sorting = Sorting,
                SearchRadius = SelectedRadius,
                EmploymentType = EmploymentType.HasValue ? (EmploymentType)EmploymentType.Value : null,
                ContractType = ContractType.HasValue ? (ContractType)ContractType.Value : null,
                NetZero = NetZero.HasValue ? Convert.ToBoolean(NetZero.Value) : null,
                Workplace = JobLocation.HasValue ? (JobLocation)JobLocation.Value : null,
                SalaryMinimum = string.IsNullOrEmpty(SalaryRange) ? null : int.Parse(SalaryRange.Split(',')[0])*1000,
                SalaryMaximum = ExtractSalaryMax(SalaryRange)
            };
            var pagedJobsResult = await _jobAppService.SearchAsync(searchInput, default);
            TotalCount = (int)pagedJobsResult.TotalCount;
            JobOffers = pagedJobsResult.Items;
        }

        private static int? ExtractSalaryMax(string salaryRange)
        {
            if (string.IsNullOrEmpty(salaryRange))
                return null;

            var salaryMax = salaryRange.Split(',');
            if (salaryMax.Length <= 1)
                return null;

            return int.Parse(salaryMax[1])*1000;
        }

        private static IEnumerable<SelectListItem> GetSalaryRanges()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem("£10,000 - £15,000", "10,15"),
                new SelectListItem("£15,000 - £20,000", "15,20"),
                new SelectListItem("£20,000 - £25,000", "20,25"),
                new SelectListItem("£25,000 - £30,000", "25,30"),
                new SelectListItem("£30,000 - £35,000", "30,35"),
                new SelectListItem("£35,000 - £40,000", "35,40"),
                new SelectListItem("£45,000 - £50,000", "45,50"),
                new SelectListItem("£55,000 - £60,000", "55,60"),
                new SelectListItem("£60,000 - £65,000", "60,65"),
                new SelectListItem("£65,000 - £70,000", "65,70"),
                new SelectListItem("£70,000 - £80,000", "70,80"),
                new SelectListItem("£80,000 - £90,000", "80,90"),
                new SelectListItem("£90,000 - £100,000", "90,100"),
                new SelectListItem("£100,000 +", "100"),
            };
        }

        private List<int> ConvertStringParameters(List<string> categories)
        {
            var convertedCategories = new List<int>();
            foreach (var item in categories)
            {
                if (int.TryParse(item, out int result))
                    convertedCategories.Add(result);
            }

            return convertedCategories;
        }
    }
}
