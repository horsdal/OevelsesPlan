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
    }
}