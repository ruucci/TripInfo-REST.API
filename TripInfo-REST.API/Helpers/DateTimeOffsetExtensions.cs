using System;
namespace TripInfoREST.API.Helpers
{
    public static class DateTimeOffsetExtensions
    {
        public static string GetCurrentAge(this DateTimeOffset dateTimeOffset)
        {
            var currentDate = DateTimeOffset.UtcNow;
            TimeSpan span = currentDate - dateTimeOffset;

            DateTime Age = DateTime.MinValue + span;
            int Years = Age.Year - 1;
            int Months = Age.Month - 1;
            int Days = Age.Day - 1;

            if(Years >=1)
            {
                if(Years == 1)
                {
                    return $"{Years} year ago";
                }
                return $"{Years} years ago";
            }
            if(Months >=1)
            {
                if (Months == 1)
                {
                    return $"{Months} month ago";
                }
                return $"{Months} months ago";
            }
            if(Days >=1)
            {
                if(Days ==1)
                {
                    return $"{Days} day ago";
                }
                return $"{Days} days ago";
            }
            return $"{Years} years, {Months} months, {Days} days ago";
        }
    }
}