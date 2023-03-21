using System;
using Volo.Abp.Domain.Entities;

namespace Collabed.JobPortal.DropDowns
{
    public class ScreeningQuestion : Entity<Guid>
    {
        public string Name { get; private set; }
        public bool? AutoRejectAnswer { get; private set; }
        public Guid JobId { get; set; }

        /* This constructor is for deserialization / ORM purpose */
        private ScreeningQuestion()
        {
        }

        public ScreeningQuestion(Guid id, Guid jobId, string name, bool? isAutoRejectQuestion) : base(id)
        {
            JobId = jobId;
            Name = name;
            AutoRejectAnswer = isAutoRejectQuestion;
        }
    }
}
