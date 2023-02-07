using System;
using System.ComponentModel.DataAnnotations;

namespace Collabed.JobPortal.Web
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class ExtendedEmailAddressAttribute : ValidationAttribute
    {
        public ExtendedEmailAddressAttribute(string errorMessage) : base(errorMessage)
        {
        }
        private static bool EnableFullDomainLiterals { get; } =
            AppContext.TryGetSwitch("System.Net.AllowFullDomainLiterals", out bool enable) ? enable : false;

        public override bool IsValid(object value)
        {
            if (value == null) // maybe edit null to be an empty string?
            {
                return true;
            }

            if (!(value is string valueAsString))
            {
                return false;
            }

            if (!EnableFullDomainLiterals && (valueAsString.Contains('\r') || valueAsString.Contains('\n')))
            {
                return false;
            }

            if (valueAsString.IndexOf('@') == 0)
            {
                return false;
            }



            var emailParts = valueAsString.Split('@');
            if (emailParts.Length != 2 || // more than one @
                emailParts[1].LastIndexOf('.') == -1 || // there is lack of '.' sign in domain
                emailParts[1].IndexOf('.') == 0 || // . is the first char in domain
                (emailParts[1].Length -1) == emailParts[1].LastIndexOf('.') || // '.' char is the last one in domain
                emailParts[0].Length > 64 ||
                emailParts[1].Length > 253)
            {
                return false;
            }

            return true;
        }
    }
}
