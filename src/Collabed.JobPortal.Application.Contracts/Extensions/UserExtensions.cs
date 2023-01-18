using Collabed.JobPortal.User;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Account;
using Volo.Abp.Data;

namespace Collabed.JobPortal.Extensions
{
    public static class UserExtensions
    {
        private const string UserTypePropertyName = "UserType";

        public static void SetUserType(this RegisterDto user, UserType userType)
        {
            user.SetProperty(UserTypePropertyName, userType);
        }

        public static string GetUserType(this RegisterDto user)
        {
            return user.GetProperty<string>(UserTypePropertyName);
        }
    }
}
