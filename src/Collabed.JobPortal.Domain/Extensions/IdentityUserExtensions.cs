using Collabed.JobPortal.User;
using Volo.Abp.Data;
using Volo.Abp.Identity;

namespace Collabed.JobPortal.Extensions
{
    public static class IdentityUserExtensions
    {
        private const string UserTypePropertyName = "UserType";
        private const string FirstNamePropertyName = "FirstName";
        private const string LastNamePropertyName = "LastName";

        public static void SetUserType(this IdentityUser user, UserType userType)
        {
            user.SetProperty(UserTypePropertyName, userType);
        }

        public static string GetUserType(this IdentityUser user)
        {
            return user.GetProperty<string>(UserTypePropertyName);
        }

        public static void SetFirstName(this IdentityUser user, string firstName)
        {
            user.SetProperty(FirstNamePropertyName, firstName);
        }
        public static void SetLastName(this IdentityUser user, string lastName)
        {
            user.SetProperty(LastNamePropertyName, lastName);
        }
    }
}
