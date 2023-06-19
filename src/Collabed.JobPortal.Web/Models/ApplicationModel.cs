using Collabed.JobPortal.Types;
using System;
using System.Collections.Generic;

namespace Collabed.JobPortal.Web.Models
{
    public class ApplicationModel
    {
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public string Tab { get; set; }
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalCount, PageSize));
        public string Reference { get; set; }
        public List<Application> Applications { get; set; } = new List<Application>();
        public bool CanHireApplicants { get; set; }
    }

    public class Application
    {
        public Guid JobApplicationId { get; set; }
        public string CandidateName { get; set; }
        public DateTime ApplicationDate { get; set; }
        public ApplicationStatus ApplicationStatus { get; set; }
        public DateTime? InterviewDate { get; set; }
        public string CandidateEmail { get; set; }
        public string CandidatePhoneNumber { get; set; }

        public string GetTextStatus()
        {
            if (ApplicationStatus == ApplicationStatus.Rejected)
                return "Rejected";

            if (ApplicationStatus == ApplicationStatus.Hired)
                return "Hired";

            return " - ";
        }
    }
}
