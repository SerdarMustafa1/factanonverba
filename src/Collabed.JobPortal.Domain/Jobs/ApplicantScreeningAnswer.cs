using System;
using Volo.Abp.Domain.Entities;

namespace Collabed.JobPortal.Jobs
{
    public class ApplicantScreeningAnswer : Entity<Guid>
    {
        public Guid JobApplicantId { get; set; }
        public Guid ScreeningQuestionId { get; private set; }
        public bool Answer { get; set; }

        private ApplicantScreeningAnswer()
        {
        }

        internal ApplicantScreeningAnswer(Guid jobApplicantId, Guid screeningQuestionId, bool answer)
        {
            Id = Guid.NewGuid();
            JobApplicantId = jobApplicantId;
            ScreeningQuestionId = screeningQuestionId;
            Answer = answer;
        }
    }
}
