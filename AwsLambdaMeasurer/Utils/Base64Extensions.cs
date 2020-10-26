using System;
using System.Text;

namespace AwsLambdaMeasurer.Utils
{
    public static class Base64Extensions
    {
        public static string ToBase64String(this string plainText)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
        }
    }
}