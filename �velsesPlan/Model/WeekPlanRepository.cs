using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ØvelsesPlan.Helpers;

namespace ØvelsesPlan.Model
{
    public class WeekPlanRepository
    {
        public WeekPlan GetCurrentWeekPlan()
        {
            return new WeekPlan(DanishClaendar.CurrentWeek);
        }

        public WeekPlan CreateWeekPlanFor(int week)
        {
           return new WeekPlan(week);
        }
    }

    public class WeekPlan : IEnumerable<WeekPlanEntry>
    {
        private readonly List<WeekPlanEntry> plan = new List<WeekPlanEntry>();
        private readonly List<Exercise> exercisesInPlan;

        public WeekPlan(int weekNumber)
        {
            WeekNumber = weekNumber;

            exercisesInPlan = new ExerciseRepository().GetAll().Shuffle().ToList();

            CreatePlan();
        }

        private void CreatePlan()
        {
            var exercisesPerDay = exercisesInPlan.Count()/5;
            var daysWithAnExtraExercise = exercisesInPlan.Count()%5;

            AddPlanFor(DayOfWeek.Monday, exercisesPerDay, daysWithAnExtraExercise);
            AddPlanFor(DayOfWeek.Tuesday, exercisesPerDay, daysWithAnExtraExercise);
            AddPlanFor(DayOfWeek.Wednesday, exercisesPerDay, daysWithAnExtraExercise);
            AddPlanFor(DayOfWeek.Thursday, exercisesPerDay, daysWithAnExtraExercise);
            AddPlanFor(DayOfWeek.Friday, exercisesPerDay, daysWithAnExtraExercise);
        }

        private void AddPlanFor(DayOfWeek day, int exercisesPerDay, int daysWithAnExtraExercise)
        {
            var exercisesForDay = ExcercisesForDayNumber(((int) day - 1), exercisesPerDay, daysWithAnExtraExercise);
            foreach (var exercise in exercisesForDay)
                plan.Add(new WeekPlanEntry(exercise, day));
        }

        private IEnumerable<Exercise> ExcercisesForDayNumber(int dayNumber, int exercisesPerDay, int daysWithAnExtraExercise)
        {
            int startIndex = dayNumber * exercisesPerDay;
            if (dayNumber < daysWithAnExtraExercise)
            {
                startIndex += dayNumber;
                exercisesPerDay += 1;
            }
            else
            {
                startIndex += daysWithAnExtraExercise;
            }
            return exercisesInPlan.GetRange(startIndex, exercisesPerDay);
        }

        public int WeekNumber { get; private set; }

        public IEnumerator<WeekPlanEntry> GetEnumerator()
        {
            return plan.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<string[]> Flatten()
        {
            return plan.Select(entry => entry.Flatten());
        }
    }
}