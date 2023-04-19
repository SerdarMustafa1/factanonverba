using System;

namespace Collabed.JobPortal.Extensions
{
    public class RandomNameGenerator
    {
        public static string GenerateRandomName(int size)
        {
            var res = new Random();

            // String that contain both alphabets and numbers
            var str = "abcdefghijklmnopqrstuvwxyz0123456789";

            // Initializing the empty string
            string randomString = "";

            for (int i = 0; i < size; i++)
            {

                // Selecting a index randomly
                int x = res.Next(str.Length);

                // Appending the character at the 
                // index to the random alphanumeric string.
                randomString += str[x];
            }

            return randomString;
        }
    }
}
