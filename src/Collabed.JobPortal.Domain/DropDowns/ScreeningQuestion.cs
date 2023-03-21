using System;
using Volo.Abp.Domain.Entities;

namespace Collabed.JobPortal.DropDowns
{
    public class ScreeningQuestion : Entity<Guid>
    {
        public string Name { get; private set; }
        public bool IsAutoRejectQuestion { get; private set; } = false;
        public Guid JobId { get; set; }

        /* This constructor is for deserialization / ORM purpose */
        private ScreeningQuestion()
        {
        }

        public ScreeningQuestion(Guid id, Guid jobId, string name, bool isAutoRejectQuestion) : base(id)
        {
            JobId = jobId;
            Name = name;
            IsAutoRejectQuestion = isAutoRejectQuestion;
        }
    }
}
