using System;
using System.Linq;
using Xunit;
using Specs;
using Should.Fluent;
using Xunit.Extensions;
using ØvelsesPlan.Model;

namespace ØvelsesPlanTests
{
    public class WeekPlanSpec : MongoIntegrationSpec
    {
        [Fact]
        public void when_database_is_empty()
        {
            new ExerciseRepository(mongoConnectionString, "OevelsesPlan").Clear();

            "The current weekplan".should(
                () =>
                    {
                        var currentWeekPlan = new WeekPlanRepository().GetCurrentWeekPlan();

                        "be for the current week".asIn
                            (
                                () => currentWeekPlan.WeekNumber.Should().Equal(DanishClaendar.CurrentWeek)
                            );

                        "contain an empty list of exercises for each weekday".asIn
                            (
                                () => currentWeekPlan.Should().Be.Empty());
                    });
        }

        [Theory,
        InlineData(1), InlineData(4), InlineData(5), InlineData(6), InlineData(7), InlineData(8),
        InlineData(42)]
        public void when_database_has_only_active_exercises(int numberOfExercisesInDatabase)
        {
            PrimeDatabaseWtih3ActiveExercises(numberOfExercisesInDatabase);
            var weekPlanRepository = new WeekPlanRepository();
            var currentWeekPlan = weekPlanRepository.GetCurrentWeekPlan();

            "The current weekplan".should(
                () =>
                    {
                        "be for the current week".asIn(
                            () => currentWeekPlan.WeekNumber.Should().Equal(DanishClaendar.CurrentWeek)
                            );

                        ("contain all " + numberOfExercisesInDatabase + " exercises").asIn(
                            () =>
                                {
                                    currentWeekPlan.Should().Count.Exactly(numberOfExercisesInDatabase);
                                    for (int i = 0; i < numberOfExercisesInDatabase; i++)
                                        currentWeekPlan.Should().Contain.Any(entry => entry.Exercise.Name == "Hop" + i);
                                }
                            );

                        "have exercises evenly distributed on weekdays".asIn(
                            () =>
                                {
                                    var approxExercisesPerDay = numberOfExercisesInDatabase/5;
                                    currentWeekPlan.Where(entry => entry.Day == DayOfWeek.Monday).Should().Count.AtLeast
                                        (approxExercisesPerDay);
                                    currentWeekPlan.Where(entry => entry.Day == DayOfWeek.Tuesday).Should().Count.
                                        AtLeast(approxExercisesPerDay);
                                    currentWeekPlan.Where(entry => entry.Day == DayOfWeek.Wednesday).Should().Count.
                                        AtLeast(approxExercisesPerDay);
                                    currentWeekPlan.Where(entry => entry.Day == DayOfWeek.Thursday).Should().Count.
                                        AtLeast(approxExercisesPerDay);
                                    currentWeekPlan.Where(entry => entry.Day == DayOfWeek.Friday).Should().Count.AtLeast
                                        (approxExercisesPerDay);
                                }
                            ).andIn(
                                () =>
                                    {
                                        currentWeekPlan.Where(entry => entry.Day == DayOfWeek.Saturday).Should().Be.
                                            Empty();
                                        currentWeekPlan.Where(entry => entry.Day == DayOfWeek.Sunday).Should().Be.Empty();
                                    }
                            );
                    });

            "and creating a new plan for the current week".should(
                () =>
                    {
                        var newCurrentWeekPlan = weekPlanRepository.CreateWeekPlanFor(DanishClaendar.CurrentWeek);
                        "be different from the old one".asIn(
                            () =>
                                {
                                    if (numberOfExercisesInDatabase > 4)
                                        newCurrentWeekPlan.SequenceEqual(currentWeekPlan).Should().Be.False();
                                });
                    });

            "and flattening the week plan".should(
                () =>
                    {
                        var flatWeekPlan = currentWeekPlan.Flatten().ToArray();
                        "result in a list of strings, the same length as the number of active exercises in the datastore".asIn(
                            () => flatWeekPlan.Should().Count.Exactly(numberOfExercisesInDatabase));
                        "each such string is a flat weekplan entry".asIn(
                            () =>
                                {
                                    var weekPlanArr = currentWeekPlan.ToArray();
                                    for (int i = 0; i < numberOfExercisesInDatabase; i++)
                                    {
                                        flatWeekPlan[i].Should().Equal(weekPlanArr[i].Flatten());
                                    }
                                });
                    });
        }

        private void PrimeDatabaseWtih3ActiveExercises(int numberOfExercisesInDatabase)
        {
            var exercises = new ExerciseRepository(mongoConnectionString, "OevelsesPlan");
            exercises.Clear();
            for (int i = 0; i < numberOfExercisesInDatabase; i++)
                exercises.Add(new Exercise(name: "Hop" + i, muscleGroup: "Lår", muscle: "Quadrozeps pemoris", active: true, description: "Foo"));
        }
    }
}