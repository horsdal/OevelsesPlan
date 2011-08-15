using Nancy;
using ØvelsesPlan.Model;

namespace ØvelsesPlan
{
    public class ExercisePlannerModule : NancyModule
    {
        private readonly WeekPlanRepository weekPlans = new WeekPlanRepository();
        private readonly ExerciseRepository exercises = new ExerciseRepository();

        public ExercisePlannerModule()
        {
            Get["/"] = _ => View["Index.htm"];
           
            Get["exercises/"] = _ => View["Exercises.htm"];
            Get["exercises/all"] = _ => Response.AsJson(new {aaData = exercises.GetAll()});
            Post["exercises/create"] = _ => Response.AsJson(exercises.Add(new Exercise(name: "Ny øvelse", muscleGroup: "muskelgruppe", muscle: "muskel", active:true, description:"beskrivelse")));
            Post["exercises/delete"] = _ => exercises.Delete(Request.Form.row_id.Value) ? HttpStatusCode.OK : HttpStatusCode.NotModified;
            Post["exercises/edit/"] = _ => EditExerciseProperty();

            Get["weekplan/current"] = _ => Response.AsJson(new { aaData = new WeekPlanRepository().GetCurrentWeekPlan().Flatten() });
            Post["weekplan/create"] = _ => Response.AsJson(new { aaData = weekPlans.CreateWeekPlanFor(DanishClaendar.CurrentWeek).Flatten() });
        }

        private Response EditExerciseProperty()
        {
            exercises.UpdateByExerciseIdByPropertyNumber((string) Request.Form.row_id.Value, (Exercise.PropertyNumbers)int.Parse(Request.Form.column.Value), (string) Request.Form.value.Value);
            return Request.Form.value.Value;
        }
    }
}