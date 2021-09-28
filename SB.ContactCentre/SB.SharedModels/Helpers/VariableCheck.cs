using System;
using System.Text.RegularExpressions;

namespace SB.SharedModels.Helpers
{
    public static class VariableCheck
    {
        public static string ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new Exception($"{nameof(phoneNumber)} is null, empty or white-space");
            }

            phoneNumber = phoneNumber.Replace("+", "");
            phoneNumber = phoneNumber.Replace(" ", "");

            if (phoneNumber.StartsWith("0"))
            {
                phoneNumber = "38" + phoneNumber;
            }

            var regex = new Regex("^(380)[1-9]{1}[0-9]{8}$");

            if(!regex.IsMatch(phoneNumber))
            {
                throw new Exception("Wrong phone number format");
            }

            return phoneNumber;
        }
    }
}