using System.Configuration;
using Nancy;
using ØvelsesPlan.Model;

namespace ØvelsesPlan
{
    public class ExercisePlannerModule : NancyModule
    {
        private readonly string connectionString;
        private const string databaseName = "OevelsesPlan";
        private readonly ExerciseRepository exercises;
        private readonly WeekPlanRepository weekPlans;

        public ExercisePlannerModule()
        {
            connectionString = ConfigurationManager.AppSettings.Get("MONGOHQ_URL");

            exercises = new ExerciseRepository(connectionString, databaseName);
            weekPlans = new WeekPlanRepository(connectionString, databaseName, exercises);

            Get["/"] = _ => View["Index.htm"];
           
            Get["exercises/"] = _ => View["Exercises.htm"];
            Get["exercises/all"] = _ => Response.AsJson(new {aaData = exercises.GetAll()});
            Post["exercises/create"] = _ => Response.AsJson(exercises.Add(new Exercise(name: "Ny øvelse", muscleGroup: "muskelgruppe", muscle: "muskel", active:true, description:"beskrivelse")));
            Post["exercises/delete"] = _ => exercises.Delete(Request.Form.row_id.Value) ? HttpStatusCode.OK : HttpStatusCode.NotModified;
            Post["exercises/edit/"] = _ => EditExerciseProperty();

            Get["weekplan/current"] = _ => Response.AsJson(new { aaData = weekPlans.GetWeekPlanFor(DanishClaendar.CurrentWeek).Flatten() });
            Post["weekplan/create"] = _ => Response.AsJson(new { aaData = weekPlans.CreateWeekPlanFor(DanishClaendar.CurrentWeek).Flatten() });
        }

        private Response EditExerciseProperty()
        {
            exercises.UpdateByExerciseIdByPropertyNumber((string) Request.Form.row_id.Value, (Exercise.PropertyNumbers)int.Parse(Request.Form.column.Value), (string) Request.Form.value.Value);
            return Request.Form.value.Value;
        }
    }
}