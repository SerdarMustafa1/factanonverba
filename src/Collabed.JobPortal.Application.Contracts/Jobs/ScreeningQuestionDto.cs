using System;
using Volo.Abp.Application.Dtos;

namespace Collabed.JobPortal.Jobs
{
    public class ScreeningQuestionDto : EntityDto<Guid>
    {
        private ScreeningQuestionDto()
        {
        }

        public ScreeningQuestionDto(Guid id, string text, bool? autoRejectAnswer = null)
        {
            Id = id;
            Text = text;
            AutoRejectAnswer = autoRejectAnswer;
        }

        public string Text { get; set; }
        public bool? AutoRejectAnswer { get; set; }
        public bool Answer { get; set; }
    }
}
