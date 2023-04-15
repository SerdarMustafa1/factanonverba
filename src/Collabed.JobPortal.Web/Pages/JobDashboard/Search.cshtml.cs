using Collabed.JobPortal.DropDowns;
using Collabed.JobPortal.Jobs;
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

        public SearchModel(DropDownAppService dropDownAppService, IJobAppService jobAppService)
        {
            _dropDownService = dropDownAppService;
            _jobAppService = jobAppService;
            AvailableLocationRadius = new SelectListItem[] {
                new SelectListItem("1 mile", "1", true),
                new SelectListItem("5 miles", "5", false),
                new SelectListItem("10 miles", "10", false)
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
            };
            var pagedJobsResult = await _jobAppService.SearchAsync(searchInput, default);
            TotalCount = (int)pagedJobsResult.TotalCount;
            JobOffers = pagedJobsResult.Items;
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
