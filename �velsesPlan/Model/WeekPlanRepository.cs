using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace ØvelsesPlan.Model
{
    public class WeekPlanRepository
    {
        private MongoServer server;
        private MongoDatabase database;
        private MongoCollection weekplanStore;
        private readonly ExerciseRepository exerciseRepository;

        public WeekPlanRepository(string connectionString, string databaseName, ExerciseRepository exerciseRepository)
        {
            server = MongoServer.Create(connectionString);
            database = server.GetDatabase(databaseName);
            weekplanStore = database.GetCollection("weekplans");
            this.exerciseRepository = exerciseRepository;
        }

        public WeekPlan GetCurrentWeekPlan()
        {
            return CreateWeekPlanFor(DanishClaendar.CurrentWeek);
        }

        public WeekPlan CreateWeekPlanFor(int week)
        {
            var weekplan = new WeekPlan(week, exerciseRepository.GetAll());
            weekplanStore.Update(
                Query.EQ("WeekNumber", week), 
                Update.Replace(new { WeekNumber = week, entries = weekplan.ToArray()}), 
                UpdateFlags.Upsert);
            return weekplan;
        }
    }
}