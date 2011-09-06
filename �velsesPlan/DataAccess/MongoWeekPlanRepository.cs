using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using ØvelsesPlan.Model;

namespace ØvelsesPlan.DataAccess
{
    public class MongoWeekPlanRepository : WeekPlanRepository
    {
        private class RawWeekPlan
        {
            public ObjectId id;
            public int weekNumber;
            public WeekPlanEntry[] entries;
        }

        private MongoDatabase database;
        private MongoCollection<RawWeekPlan> weekplanStore;
        private readonly ExerciseRepository exerciseRepository;

        public MongoWeekPlanRepository(string connectionString, ExerciseRepository exerciseRepository)
        {
            database = MongoDatabase.Create(connectionString);
            weekplanStore = database.GetCollection<RawWeekPlan>("weekplans");
            this.exerciseRepository = exerciseRepository;
        }

        public WeekPlan CreateWeekPlanFor(int week)
        {
            var weekplan = new WeekPlan(week, exerciseRepository.GetAllActive());
            var rawWeekPlan = new RawWeekPlan {id = CreateObjectId(week),  weekNumber = week, entries = weekplan.ToArray()};
            weekplanStore.Update(
                Query.EQ("weekNumber", week),
                Update.Replace(rawWeekPlan),
                UpdateFlags.Upsert);
            return weekplan;
        }

        private ObjectId CreateObjectId(int week)
        {
            return new ObjectId(
                new byte[] {0,0,0,0,0,0,0,0,0,0, (byte) (week / 10),(byte) (week % 10) });
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
