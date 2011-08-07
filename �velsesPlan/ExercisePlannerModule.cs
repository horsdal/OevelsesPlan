using System;
using System.Collections.Generic;
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
            var exerciseRepo = new ExerciseRepository();

            Get["/"] = _ => View["Index.htm"];
            Get["exercises/"] = _ => View["Exercises.htm"];
            Get["exercises/all"] = _ => CreateJsonResponseFor(exerciseRepo.GetAll());
                                      
            Get["weekplan/current"] = _ => CreateJsonResponseFor(new WeekPlanRepository().GetCurrentWeekPlan());

            Post["exercises/create"] = _ => Response.AsJson(exerciseRepo.Add(new Exercise(name: "Ny øvelse", muscleGroup: "muskelgruppe", muscle: "muskel", active:true, description:"beskrivelse")).Flatten());
            Post["exercises/delete"] = _ => "deleting";
            Post["exercises/edit/"] = _ => { return Request.Form.value; };
            Post["weekplan/create"] = _ => CreateJsonResponseFor(weekPlans.CreateWeekPlanFor(DanishClaendar.CurrentWeek));
        }

        private Response CreateJsonResponseFor(IEnumerable<Exercise> exercises)
        {
            var flatExerciseArray = exercises
                .Select(exercise => exercise.Flatten())
                .ToArray();

            return Response.AsJson(new {aaData = flatExerciseArray});
        }

        private Response CreateJsonResponseFor(WeekPlan currentWeekPlan)
        {
            var flatWeekPlan = currentWeekPlan
                .Select(entry => entry.Flatten())
                .ToArray();

            return
                Response.AsJson(new { aaData = flatWeekPlan});
        }
    }
}