using System;

namespace ØvelsesPlan.Model
{
    public class WeekPlanEntry
    {
        public WeekPlanEntry(Exercise exercise, DayOfWeek day)
        {
            Exercise = exercise;
            Day = day;
            
        }

        public Exercise Exercise { get; private set; }
        public DayOfWeek Day { get; private set; }

        public override bool Equals(object obj)
        {
            var rhs = obj as WeekPlanEntry;
            return rhs != null && this.Exercise.Name == rhs.Exercise.Name && this.Day == rhs.Day;
        }

        public string[] Flatten()
        {
            return new[]
                       {
                           DanishClaendar.WeekDayNameFor(Day),
                           Exercise.Name,
                           Exercise.Description
                       };
        }
    }
}