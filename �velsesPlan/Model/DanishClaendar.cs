using System;
using System.Globalization;

namespace ØvelsesPlan.Model
{
    public static class DanishClaendar
    {
        private static readonly CultureInfo Danish = new CultureInfo("da-DK");
        public static int CurrentWeek
        {
            get
            {
                return Danish.Calendar.GetWeekOfYear(
                    DateTime.Now, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            }
        }

        public static string WeekDayNameFor(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Sunday:
                    return "Søndag";
                case DayOfWeek.Monday:
                    return "Mandag";
                case DayOfWeek.Tuesday:
                    return "Tirsdag";
                case DayOfWeek.Wednesday:
                    return "Onsdag";
                case DayOfWeek.Thursday:
                    return "Torsdag";
                case DayOfWeek.Friday:
                    return "Fredag";
                case DayOfWeek.Saturday:
                    return "Lørdag";
                default:
                    throw new ArgumentOutOfRangeException("day");
            }
        }
    }
}