using Collabed.JobPortal.User;
using Volo.Abp.Account;
using Volo.Abp.Data;

namespace Collabed.JobPortal.Extensions
{
    public static class UserExtensions
    {
        private const string UserTypePropertyName = "UserType";
        private const string FirstNamePropertyName = "FirstName";
        private const string LastNamePropertyName = "LastName";
        private const string OrganisationNamePropertyName = "Organisation";

        public static void SetUserType(this RegisterDto user, UserType userType)
        {
            user.SetProperty(UserTypePropertyName, userType);
        }
        public static void SetFirstName(this RegisterDto user, string firstName)
        {
            user.SetProperty(FirstNamePropertyName, firstName);
        }
        public static void SetLastName(this RegisterDto user, string lastName)
        {
            user.SetProperty(LastNamePropertyName, lastName);
        }
        public static void SetOrganisationName(this RegisterDto user, string organisationName)
        {
            user.SetProperty(OrganisationNamePropertyName, organisationName);
        }
        public static string GetFirstName(this RegisterDto user)
        {
            return user.GetProperty<string>(FirstNamePropertyName);
        }
        public static string GetLastName(this RegisterDto user)
        {
            return user.GetProperty<string>(LastNamePropertyName);
        }
        public static string GetOrganisationName(this RegisterDto user)
        {
            return user.GetProperty<string>(OrganisationNamePropertyName);
        }
        public static UserType GetUserType(this RegisterDto user)
        {
            return user.GetProperty<UserType>(UserTypePropertyName);
        }
    }
}
