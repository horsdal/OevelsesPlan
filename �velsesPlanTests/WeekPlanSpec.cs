﻿using System;
using System.Linq;
using MongoDB.Driver.Builders;
using Ploeh.AutoFixture.Xunit;
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
            var exercises = new ExerciseRepository(mongoConnectionString, databaseName);
            exercises.Clear();

            "The current weekplan".should(
                () =>
                    {
                        var currentWeekPlan = new WeekPlanRepository(mongoConnectionString, databaseName, exercises).GetWeekPlanFor(DanishClaendar.CurrentWeek);

                        "be for the current week".asIn
                            (
                                () => currentWeekPlan.WeekNumber.Should().Equal(DanishClaendar.CurrentWeek)
                            );

                        "contain an empty list of exercises for each weekday".asIn
                            (
                                () => currentWeekPlan.Should().Be.Empty()
                            );
                    });
        }

        [Theory,
        InlineData(1), InlineData(4), InlineData(5), InlineData(6), InlineData(7), InlineData(8),
        InlineData(42)]
        public void when_database_has_only_active_exercises(int numberOfExercisesInDatabase)
        {
            PrimeDatabaseWtih3ActiveExercises(numberOfExercisesInDatabase);
            var weekPlanRepository = new WeekPlanRepository(mongoConnectionString, databaseName, new ExerciseRepository(mongoConnectionString, databaseName));
            var currentWeekPlan = weekPlanRepository.GetWeekPlanFor(DanishClaendar.CurrentWeek);

            "The current weekplan".should(
                () =>
                    {
                        "be for the current week".asIn(
                            () => currentWeekPlan.WeekNumber.Should().Equal(DanishClaendar.CurrentWeek)
                            );

                        ("contain all (" + numberOfExercisesInDatabase + ") exercises").asIn(
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
                        "not change when retrieved again".asIn(
                            () =>
                                {
                                    var reRetrivedcurrentWeekPlan = weekPlanRepository.GetWeekPlanFor(DanishClaendar.CurrentWeek);
                                    reRetrivedcurrentWeekPlan.Should().Equal(currentWeekPlan);
                                }
                            );
                    });

            "and explicitly creating a new plan for the current week".should(
                () =>
                    {
                        var newCurrentWeekPlan = weekPlanRepository.CreateWeekPlanFor(DanishClaendar.CurrentWeek);
                        "create a different one from the old one".asIn(
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
                        "and each such string is a flat weekplan entry".asIn(
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

        [Theory, AutoData]
        public void the_weekplan_repository(int weekNumber)
        {
            var weekplanRepo = new WeekPlanRepository(mongoConnectionString, databaseName, new ExerciseRepository(mongoConnectionString, databaseName));

            "Be backed by a mongo db".asIn(
                            () =>
                            {
                                var database = mongoServer.GetDatabase(databaseName);
                                var weekplan = weekplanRepo.CreateWeekPlanFor(weekNumber);

                                var weekplanCollectionFromMongo = database.GetCollection("weekplans");
                                var weekplanRetrivedFromMonngo = weekplanCollectionFromMongo.Find(Query.EQ("weekNumber", weekplan.WeekNumber)).FirstOrDefault();

                                weekplanRetrivedFromMonngo.Should().Not.Be.Null();
                                weekplanRetrivedFromMonngo["weekNumber"].AsInt32.Should().Equal(weekNumber);
                                weekplanRetrivedFromMonngo["entries"].AsBsonArray.Should().Count.Exactly(weekplan.Count());
                            });
        }

        private void PrimeDatabaseWtih3ActiveExercises(int numberOfExercisesInDatabase)
        {
            var exercises = new ExerciseRepository(mongoConnectionString, databaseName);
            exercises.Clear();
            for (int i = 0; i < numberOfExercisesInDatabase; i++)
                exercises.Add(new Exercise(name: "Hop" + i, muscleGroup: "Lår", muscle: "Quadrozeps pemoris", active: true, description: "Foo"));
        }
    }
}