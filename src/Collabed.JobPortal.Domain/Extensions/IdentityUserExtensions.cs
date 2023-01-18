using Collabed.JobPortal.User;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Data;
using Volo.Abp.Identity;

namespace Collabed.JobPortal.Extensions
{
    public static class IdentityUserExtensions
    {
        private const string UserTypePropertyName = "UserType";

        public static void SetUserType(this IdentityUser user, UserType userType)
        {
            user.SetProperty(UserTypePropertyName, userType);
        }

        public static string GetUserType(this IdentityUser user)
        {
            return user.GetProperty<string>(UserTypePropertyName);
        }
    }
}
