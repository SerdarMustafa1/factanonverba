using Collabed.JobPortal.Web.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Collabed.JobPortal.Web.Attributes
{
    public class RequiredWhenPersonalAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                var manageModel = (IndividualAccountModel)validationContext.ObjectInstance;
                if (manageModel == null || manageModel.UserType == null || manageModel.UserType != User.UserType.Candidate)
                {
                    return ValidationResult.Success;
                }
            }
            catch (System.Exception)
            {
                return ValidationResult.Success;
            }
            var property = validationContext.ObjectType.GetProperty(validationContext.MemberName);
            var attribute = property.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();

            string propertyName;
            if (attribute != null)
            {
                propertyName = ((DisplayAttribute)attribute).Name;
            }
            else
            {
                propertyName = property.Name;
            }
            return string.IsNullOrWhiteSpace(value as string)
                ? new ValidationResult($"{propertyName} is required.")
                : ValidationResult.Success;
        }
    }
}
