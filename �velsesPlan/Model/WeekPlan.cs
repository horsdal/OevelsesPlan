using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using ØvelsesPlan.Helpers;

namespace ØvelsesPlan.Model
{
    public class WeekPlan : IEnumerable<WeekPlanEntry>
    {
        private readonly List<WeekPlanEntry> plan = new List<WeekPlanEntry>();
        private readonly List<Exercise> exercisesInPlan;

        public int WeekNumber { get; private set; }
        public ObjectId Id { get; private set; }

        public WeekPlan(int weekNumber, IEnumerable<Exercise> exercisesToInclude)
        {
            WeekNumber = weekNumber;
            Id = new ObjectId();

            exercisesInPlan = exercisesToInclude.Shuffle().ToList();

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


        public IEnumerator<WeekPlanEntry> GetEnumerator()
        {
            return plan.GetEnumerator();
        }

        public IEnumerable<string[]> Flatten()
        {
            return plan.Select(entry => entry.Flatten());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}