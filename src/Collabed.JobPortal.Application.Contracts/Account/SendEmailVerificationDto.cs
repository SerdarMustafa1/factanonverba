namespace Collabed.JobPortal.Account
{
    public class SendEmailVerificationDto
    {
        public string Email { get; set; }
        public string CallbackUrl { get; set; }
    }
}
