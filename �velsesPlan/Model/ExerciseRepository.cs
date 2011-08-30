using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace ØvelsesPlan.Model
{
    public class ExerciseRepository
    {
        private MongoDatabase database;
        private MongoCollection<Exercise> exerciseStore;

        public ExerciseRepository(string connectionString)
        {
           database = MongoDatabase.Create(connectionString);
            exerciseStore = database.GetCollection<Exercise>("exercises");
        }

        public Exercise Add(Exercise exercise)
        {
            var succes = exerciseStore.Insert(exercise, SafeMode.True);
            return exercise;
        }

        public IEnumerable<Exercise> GetAll()
        {
            return exerciseStore.FindAll();
        }

        public void Clear()
        {
            exerciseStore.RemoveAll();
        }

        public Exercise GetById(string id)
        {
            var exerciseToFind = exerciseStore.Find(Query.EQ("_id", id)).FirstOrDefault();
            return exerciseToFind != null ? new Exercise(id, exerciseToFind.Name, exerciseToFind.MuscleGroup, exerciseToFind.Muscle, exerciseToFind.Active, exerciseToFind.Description) : null;
        }

        public void Update(Exercise updatedExercise)
        {
            Delete(updatedExercise.Id);
            Add(updatedExercise);
        }

        public void UpdateByExerciseIdByPropertyNumber(string exerciseId, Exercise.PropertyNumbers columnNumber, string newValue)
        {
            var exerciseToUpdate = GetById(exerciseId);
            exerciseToUpdate.UpdatePropertyNumber(columnNumber, newValue);
            Update(exerciseToUpdate);
        }

        public bool Delete(string id)
        {
            var res = exerciseStore.Remove(Query.EQ("_id", id));
            //return res.Ok;
            return true;
        }

        public IEnumerable<Exercise> GetAllActive()
        {
            return exerciseStore.Find(Query.EQ("Active", true));
        }
    }
}