using System;
using System.Configuration;
using Nancy;
using ØvelsesPlan.DataAccess;
using ØvelsesPlan.Model;

namespace ØvelsesPlan
{
    public class ExercisePlannerModule : NancyModule
    {
        private readonly string connectionString;
        private readonly ExerciseRepository exercises;
        private readonly WeekPlanRepository weekPlans;

        public ExercisePlannerModule()
        {
            connectionString = ConfigurationManager.AppSettings.Get("MONGOHQ_URL");

            exercises = new MongoExerciseRepository(connectionString);
            weekPlans = new WeekPlanRepository(connectionString, exercises);

            Get["/"] = _ => View["Index.htm"];

            Get["exercises/"] = _ => View["Exercises.htm"];
            Get["exercises/all"] = _ => Response.AsJson(new { aaData = exercises.GetAll() });
            Post["exercises/create"] = _ => Response.AsJson(exercises.Add(new Exercise(name: "Ny øvelse", muscleGroup: "muskelgruppe", muscle: "muskel", active: true, description: "beskrivelse")));
            Post["exercises/delete"] = _ => exercises.Delete(Request.Form.row_id.Value) ? HttpStatusCode.OK : HttpStatusCode.NotModified;
            Post["exercises/edit/"] = _ => EditExerciseProperty();

            Get["weekplan/current"] = _ => GetWeekPlanFor(DanishClaendar.CurrentWeek);
            Get[@"weekplan/(?<weeknumber>[\d]{1,2})"] = _ => GetWeekPlanFor(_.weeknumber);
            Post["weekplan/create"] = _ => CreateWeekPlanFor(Request.Form.weeknumber);
        }

        private Response EditExerciseProperty()
        {
            exercises.UpdateByExerciseIdByPropertyNumber((string)Request.Form.row_id.Value, (Exercise.PropertyNumbers)int.Parse(Request.Form.column.Value), (string)Request.Form.value.Value);
            return Request.Form.value.Value;
        }

        private Response CreateWeekPlanFor(int weekNumber)
        {
            return Response.AsJson(
                new
                {
                    weekNumber,
                    aaData = weekPlans.CreateWeekPlanFor(weekNumber).Flatten()
                });
        }

        private Response GetWeekPlanFor(int weekNumber)
        {
            return Response.AsJson(
                new
                    {
                        weekNumber,
                        aaData = weekPlans.GetWeekPlanFor(weekNumber).Flatten()
                    });
        }
    }
}