using System;

namespace SB.Shared.Helpers
{
    public static class ParameterCheckHelper
    {
        public static void ValidateStringParameter(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new Exception($"{parameterName} is null, empty or whitespace");
            }
        }
    }
}