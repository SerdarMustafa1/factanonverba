using Collabed.JobPortal.Types;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Collabed.JobPortal.Web.Models
{
    public class ApplicationModel
    {
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public string Tab { get; set; }
        [CanBeNull] public string SortBy { get; set; }
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalCount, PageSize));
        public string Reference { get; set; }
        public List<Application> Applications { get; set; } = new List<Application>();
        public bool CanHireApplicants { get; set; }
        public IEnumerable<SelectListItem> Ratings { get; } = new SelectListItem[] {
                new SelectListItem("1", "1", true),
                new SelectListItem("2", "2", true),
                new SelectListItem("3", "3", true),
                new SelectListItem("4", "4", true),
                new SelectListItem("5", "5", true)
            };
    }

    public class Application
    {
        public string Reference { get; set; }
        public Guid JobApplicationId { get; set; }
        public string CandidateName { get; set; }
        public DateTime ApplicationDate { get; set; }
        public ApplicationStatus ApplicationStatus { get; set; }
        public DateTime? InterviewDate { get; set; }
        public string CandidateEmail { get; set; }
        public string CandidatePhoneNumber { get; set; }
        public int? Rating { get; set; }

        public string GetTextStatus()
        {
            if (ApplicationStatus == ApplicationStatus.New)
                return " - ";

            return ApplicationStatus.ToString();
        }
    }
}
