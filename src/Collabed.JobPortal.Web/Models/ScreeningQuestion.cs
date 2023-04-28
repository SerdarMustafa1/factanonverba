using Collabed.JobPortal.Jobs;

namespace Collabed.JobPortal.Web.Models
{
    public class ScreeningQuestion 
    {
        public string Question { get; set; }
        public bool? AutoRejectAnswer { get; set; }

        public ScreeningQuestion(string question, bool? autoRejectAnswer)
        {
            Question = question;
            AutoRejectAnswer = autoRejectAnswer;
        }

        public ScreeningQuestion()
        {

        }
    }
}
