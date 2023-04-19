using Collabed.JobPortal.Types;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Collabed.JobPortal.Jobs
{
    public class JobApplicant : Entity
    {
        public Guid JobApplicantId { get; private set; }
        public Guid UserId { get; private set; }
        public Guid JobId { get; private set; }
        public string CvBlobName { get; set; }
        public string CvFileName { get; set; }
        public string CvContentType { get; set; }
        public string CoverLetter { get; set; }
        public string Portfolio { get; set; }
        public ApplicationStatus ApplicationStatus { get; set; }
        public DateTime ApplicationDate { get; private set; }
        public DateTime? InterviewDate { get; set; }
        public bool StatusChangePublished { get; set; }
        public bool NotificationSent { get; set; }
        public IEnumerable<ApplicantScreeningAnswer> ApplicantScreeningAnswers { get; private set; }

        /* This constructor is for deserialization / ORM purpose */
        private JobApplicant()
        {
            JobApplicantId = Guid.NewGuid();
        }

        internal JobApplicant(Guid userId, Guid jobId)
        {
            JobApplicantId = Guid.NewGuid();
            UserId = userId;
            JobId = jobId;
            ApplicationStatus = ApplicationStatus.New;
            ApplicationDate = DateTime.Now;
        }

        public override object[] GetKeys()
        {
            return new object[] { UserId, JobId };
        }

        public JobApplicant SetApplicantScreeningAnswers(IEnumerable<(Guid, bool)> answers)
        {
            var applicantScreeningAnswers = new List<ApplicantScreeningAnswer>();
            foreach (var (questionId, answer) in answers)
            {
                applicantScreeningAnswers.Add(new ApplicantScreeningAnswer(JobApplicantId, questionId, answer));
            }
            ApplicantScreeningAnswers = applicantScreeningAnswers;

            return this;
        }
    }
}
