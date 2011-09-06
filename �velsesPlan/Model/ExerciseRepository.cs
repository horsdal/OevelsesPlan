using System;
using System.Collections.Generic;

namespace ØvelsesPlan.Model
{
    public interface ExerciseRepository
    {
        Exercise Add(Exercise exercise);
        IEnumerable<Exercise> GetAll();
        void Clear();
        Exercise GetById(string id);
        void Update(Exercise updatedExercise);
        void UpdateByExerciseIdByPropertyNumber(string exerciseId, Exercise.PropertyNumbers columnNumber, string newValue);
        bool Delete(string id);
        IEnumerable<Exercise> GetAllActive();
    }
}