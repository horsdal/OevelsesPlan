using System;
using System.Collections;
using System.Collections.Generic;

namespace ØvelsesPlan.Model
{
    public class ExerciseRepository
    {
        private static readonly List<Exercise> Exercises = new List<Exercise>();

        public Exercise Add(Exercise exercise)
        {
            Exercises.Add(exercise);
            return exercise;
        }

        public IEnumerable<Exercise> GetAll()
        {
            return Exercises;
        }

        public void Clear()
        {
            Exercises.Clear();
        }

        public Exercise GetById(string id)
        {
            var exerciseToFind = Exercises.Find(e => e.Id == id);
            return exerciseToFind != null ? new Exercise(exerciseToFind.Name, exerciseToFind.MuscleGroup, exerciseToFind.Muscle, exerciseToFind.Active, exerciseToFind.Description) : null;
        }

        public void Update(Exercise updatedExercise)
        {
            Exercises.RemoveAll(e => e.Id == updatedExercise.Id);
            Add(updatedExercise);
        }

        public void Delete(Exercise exercise)
        {
            Exercises.Remove(exercise);
        }
    }
}