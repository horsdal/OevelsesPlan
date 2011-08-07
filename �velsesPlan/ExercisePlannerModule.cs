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
            Get["exercises/all"] = _ => Response.AsJson(new {aaData = exerciseRepo.GetAll()});
                                      
            Get["weekplan/current"] = _ => Response.AsJson(new { aaData = new WeekPlanRepository().GetCurrentWeekPlan().Flatten()});

            Post["exercises/create"] = _ => Response.AsJson(exerciseRepo.Add(new Exercise(name: "Ny øvelse", muscleGroup: "muskelgruppe", muscle: "muskel", active:true, description:"beskrivelse")));
            Post["exercises/delete"] = _ => "deleting";
            Post["exercises/edit/"] = _ => { return Request.Form.value.Value; };
            Post["weekplan/create"] = _ => Response.AsJson(new { aaData = weekPlans.CreateWeekPlanFor(DanishClaendar.CurrentWeek).Flatten()});
        }
    }
}