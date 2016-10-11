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
        public static XAxis GetRecency(this DateTime value, RecencyTypes recencyType)
        {
            int number;

            var currentYear = new DateTime(value.Year, 1, 1);
            switch (recencyType)
            {
               case RecencyTypes.Weekly:
                    
                    number =   CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(value, CalendarWeekRule.FirstFullWeek,
                        DayOfWeek.Saturday);
                   
                    var dateOfWeek = currentYear.AddDays(number*7);

                    dateOfWeek = dateOfWeek.AddDays(DayOfWeek.Monday - dateOfWeek.DayOfWeek);
                   
                    return new XAxis
                    {
                        RecencyNumber = (long)((currentYear.Subtract(new DateTime(2014, 1, 1)).TotalDays/7) + number),
                        Lable = dateOfWeek.ToString("dd/MM/yyyy"),
                    };
                   
                case RecencyTypes.Monthly:

                    number = CultureInfo.InvariantCulture.Calendar.GetMonth(value);
                    currentYear = new DateTime(value.Year, 1, 1);
                    return new XAxis
                    {
                        RecencyNumber = (long)((currentYear.Subtract(new DateTime(2014, 1, 1)).TotalDays / 30) + number),
                        Lable = new DateTime(value.Year, number, 1).ToString("MMM", CultureInfo.InvariantCulture),
                    };
                  

                case RecencyTypes.Quarterly:

                    number = (int) Math.Ceiling(CultureInfo.InvariantCulture.Calendar.GetMonth(value)/3d);
                    return new XAxis
                    {
                        RecencyNumber = (long)((currentYear.Subtract(new DateTime(2014, 1, 1)).TotalDays / 90) + number),
                        Lable = $"FY{value.Year}Q{number}",
                    };

                case RecencyTypes.Fortnightly:

                    number =(int)Math.Ceiling(CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(value, CalendarWeekRule.FirstFullWeek,
                                DayOfWeek.Saturday)/2d);

                    currentYear = new DateTime(value.Year, 1, 1);
                    dateOfWeek = currentYear.AddDays(number * 14);

                    return new XAxis
                    {
                        RecencyNumber = (long)((currentYear.Subtract(new DateTime(2014, 1, 1)).TotalDays / 14) + number),
                        Lable = dateOfWeek.ToString("dd/MM/yyyy"),
                    };

            }

            return null;
        }
    }
}
