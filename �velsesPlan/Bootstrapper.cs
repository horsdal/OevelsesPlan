using System.Configuration;
using Nancy;
using ØvelsesPlan.DataAccess;
using ØvelsesPlan.Model;

namespace ØvelsesPlan
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoC.TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            var connectionString = ConfigurationManager.AppSettings.Get("MONGOHQ_URL");

            container.Register<ExerciseRepository>(
                (c, p) => new MongoExerciseRepository(connectionString));

            container.Register<WeekPlanRepository>(
                (c, p) =>
                new MongoWeekPlanRepository(connectionString, c.Resolve<ExerciseRepository>()));
        }
    }
}