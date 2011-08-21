using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace ØvelsesPlan.Model
{
    public class WeekPlanRepository
    {
        private class RawWeekPlan
        {
            public ObjectId id;
            public int weekNumber;
            public WeekPlanEntry[] entries;
        }

        private MongoServer server;
        private MongoDatabase database;
        private MongoCollection<RawWeekPlan> weekplanStore;
        private readonly ExerciseRepository exerciseRepository;

        public WeekPlanRepository(string connectionString, string databaseName, ExerciseRepository exerciseRepository)
        {
            server = MongoServer.Create(connectionString);
            database = server.GetDatabase(databaseName);
            weekplanStore = database.GetCollection<RawWeekPlan>("weekplans");
            this.exerciseRepository = exerciseRepository;
        }

        public WeekPlan CreateWeekPlanFor(int week)
        {
            var weekplan = new WeekPlan(week, exerciseRepository.GetAll());
            var rawWeekPlan = new RawWeekPlan {weekNumber = week, entries = weekplan.ToArray()};
            weekplanStore.Update(
                Query.EQ("weekNumber", week),
                Update.Replace(rawWeekPlan),
                UpdateFlags.Upsert);
            return weekplan;
        }

        public WeekPlan GetWeekPlanFor(int week)
        {
            var rawWeekPlan = weekplanStore.Find(Query.EQ("weekNumber", week)).FirstOrDefault();
            return rawWeekPlan != null
                       ? new WeekPlan(rawWeekPlan.weekNumber, rawWeekPlan.entries)
                       : CreateWeekPlanFor(week);
        }
    }
}
