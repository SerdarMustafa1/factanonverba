using Volo.Abp.Application.Dtos;

namespace Collabed.JobPortal.Jobs
{
    public class ScreeningQuestionDto : EntityDto<int>
    {
        public string Text { get; set; }
        public bool? AutoRejectAnswer { get; set; }
        public bool Answer { get; set; }
    }
}
