using System;
using System.Collections;
using System.Collections.Generic;

namespace ØvelsesPlan.Model
{
    public class ExerciseRepository
    {
        private static readonly List<Exercise> Exercises = new List<Exercise>();

        public void Create(Exercise exercise)
        {
            Exercises.Add(exercise);
        }

        public IEnumerable<Exercise> GetAll()
        {
            return Exercises;
        }

        public void Clear()
        {
            Exercises.Clear();
        }
    }
}