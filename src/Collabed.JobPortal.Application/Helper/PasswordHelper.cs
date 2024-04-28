using Azure;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collabed.JobPortal.Helper
{
    public static class PasswordHelper
    {
        public static string GenerateRandomPassword()
        {
            int length = 8;
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            string password = "";
            for (int i = 0; i < length; i++)
            {
                password += chars[random.Next(chars.Length)];
            }

            var newPassword = $"{password}8!";
            return newPassword;
        }
    }
}
