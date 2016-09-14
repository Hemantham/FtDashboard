using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.API.Enums;
using Dashboard.API.Models;

namespace Dashboard.Services.Utilities
{
    public static class DateTimeExtensions
    {
        public static Recency GetRecency(this DateTime value, RecencyType recencyType)
        {
            int number;
            switch (recencyType)
            {
               case RecencyType.Weekly:
                    number = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(value, CalendarWeekRule.FirstFullWeek,
                        DayOfWeek.Monday);

                    DateTime dateOfWeek = new DateTime(value.Year, 1, 1).AddDays(number*7);

                    return new Recency
                    {
                        RecencyNumber = number,
                        Lable = dateOfWeek.AddDays(DayOfWeek.Monday - dateOfWeek.DayOfWeek).ToShortDateString(),
                    };

                case RecencyType.Monthly:

                    number = CultureInfo.InvariantCulture.Calendar.GetMonth(value);
                    return new Recency
                    {
                        RecencyNumber = number,
                        Lable = new DateTime(value.Year, number, 1).ToString("MMM", CultureInfo.InvariantCulture),
                    };
                  

                case RecencyType.Quarterly:

                    number = (int) Math.Ceiling(CultureInfo.InvariantCulture.Calendar.GetMonth(value)/3d);
                    return new Recency
                    {
                        RecencyNumber = number,
                        Lable = $"FY{value.Year}Q{number}",
                    };

                case RecencyType.Fortnightly:

                    number =(int)Math.Ceiling(CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(value, CalendarWeekRule.FirstFullWeek,
                                DayOfWeek.Monday)/2d);

                    dateOfWeek = new DateTime(value.Year, 1, 1).AddDays(number * 14);

                    return new Recency
                    {
                        RecencyNumber = number,
                        Lable = dateOfWeek.AddDays(DayOfWeek.Monday - dateOfWeek.DayOfWeek).ToShortDateString(),
                    };

            }

            return null;
        }
    }
}
