using System;
using System.Globalization;
using SB.Shared.Models.Actions;

namespace SB.Actions.Messages
{
    public class CurrentDateTime : IActionTracking
    {
        public void Execute(string parameters, ref ActionResponse actionResponse)
        {
            DateTime dateTime;
            const string timeZone = "FLE Standard Time";

            try
            {
                var timeZoneId = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                dateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneId);
            }
            catch (TimeZoneNotFoundException)
            {
                throw new Exception($"Unable to find the {timeZone} zone in the registry.");
            }
            catch (InvalidTimeZoneException)
            {
                throw new Exception($"Registry data on the {timeZone} zone has been corrupted.");
            }

            actionResponse.Value = dateTime.ToString(CultureInfo.InvariantCulture);
        }
    }
}