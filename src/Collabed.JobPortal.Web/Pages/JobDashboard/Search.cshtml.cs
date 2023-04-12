using Collabed.JobPortal.DropDowns;
using Collabed.JobPortal.Jobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
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
        public int TotalCount { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Category { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Predicate { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Location { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SelectedRadius { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedSortingType { get; set; }

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
            Categories.FirstOrDefault(c => c.Text.Equals(Category));
            var searchInput = new SearchCriteriaInput()
            {
                CategoryId = Category,
                Keyword = string.IsNullOrWhiteSpace(Predicate) ? string.Empty : Predicate,
                MaxResultCount = 1000,
                SkipCount = 0,
                Location = string.IsNullOrWhiteSpace(Location) ? string.Empty : Location,
                Sorting = SelectedSortingType,
                SearchRadius = SelectedRadius
            };
            var pagedJobsResult = await _jobAppService.SearchAsync(searchInput, default);
            TotalCount = (int)pagedJobsResult.Items.Count;
            JobOffers = pagedJobsResult.Items;
        }

        public async Task<IActionResult> OnPostAsync() 
        {
            throw new System.NotImplementedException();
        }
    }
}
