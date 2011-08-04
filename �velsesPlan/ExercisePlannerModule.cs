using System;
using System.Linq;
using Nancy;
using ØvelsesPlan.Model;

namespace ØvelsesPlan
{
    public class ExercisePlannerModule : NancyModule
    {
        public ExercisePlannerModule()
        {
            var weekPlans = new WeekPlanRepository();

            Get["/"] = _ => View["Index.htm"];
            Get["exercises/"] = _ => View["Exercises.htm"];
            Get["exercises/all"] = _ => 
                                       {
                                           var res =
                                               Response.AsJson(
                                               new
                                               {
                                                   aaData =
                                                       new[]
                                                       {
                                                           new[] {"Hop", "Ryg", "Trapezius", "Ja"}, 
                                                           new[] {"Dans", "Lår", "Quadrozeps pemoris", "Ja"},
                                                           new[] {"Syng", "Sind", "Humør", "Nej"}
                                                       }
                                               });
                                           return res;
                                       };
            Get["exercises/create"] = _ =>
                                          {
                                              var exercises = new ExerciseRepository();
                                              exercises.Clear();
                                              for (int i = 0; i < 66; i++)
                                                  exercises.Create(new Exercise(name: "Hop" + i, muscleGroup: "Lår",
                                                                                muscle: "Quadrozeps pemoris",
                                                                                active: true));
                                              return View["CreateExercise.htm"];
                                          };
            Get["weekplan/current"] = _ =>
                                          {
                                              var currentWeekPlan = new WeekPlanRepository().GetCurrentWeekPlan();
                                              var flatWeekPlan = currentWeekPlan
                                                  .Select(entry =>
                                                          new[]
                                                              {
                                                                  DanishClaendar.WeekDayNameFor(entry.Day),
                                                                  entry.Exercise.Name
                                                              })
                                                  .ToArray();

                                              return
                                                  Response.AsJson(new { aaData = flatWeekPlan});
                                          };

            Post["exercises/create"] = _ => "creating...";
            Post["exercises/delete"] = _ => "deleting";
            Post["exercises/edit"] = _ => "editing";
            Post["weekplan/create"] = _ => "alert(\"creating weekplan\")";
        }
    }
}