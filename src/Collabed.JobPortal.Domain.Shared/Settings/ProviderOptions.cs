using System;
using System.Collections.Generic;
using System.Text;

namespace Collabed.JobPortal.Settings
{
    public class ProviderOptions
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ReturnUrl { get; set; }
        public string ClaimsIssuer { get; set; }
        public string CallbackPath { get; set; }
    }
}
