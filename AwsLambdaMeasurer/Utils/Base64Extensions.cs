using System;
using System.Text;

namespace AwsLambdaMeasurer.Utils
{
    public static class Base64Extensions
    {
        public static string EncodeBase64(this string plainText)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
        }

        public static string DecodeBase64(this string encodedString)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encodedString));
        }
    }
}