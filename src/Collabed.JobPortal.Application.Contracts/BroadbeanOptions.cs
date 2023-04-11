namespace Collabed.JobPortal
{
    public class BroadbeanOptions : DefaultCredentialOptions { }

    public class IdibuOptions : DefaultCredentialOptions { }

    public class DefaultCredentialOptions
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
