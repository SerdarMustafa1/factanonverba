namespace Collabed.JobPortal.Web.Models
{
    public class EmailTemplatesModel
    {
        public string CallbackUrl { get; }

        public EmailTemplatesModel(string callbackUrl)
        {
            CallbackUrl = callbackUrl;
        }
    }
}
