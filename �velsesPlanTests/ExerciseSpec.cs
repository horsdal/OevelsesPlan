using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Ploeh.AutoFixture.Xunit;
using Should.Fluent;
using Specs;
using Xunit.Extensions;
using ØvelsesPlan.Model;

namespace ØvelsesPlanTests
{
    public class ExerciseSpec : MongoIntegrationSpec
    {
        [Theory, AutoData]
        public void given_a_database(Exercise inputExercise)
        {
            "The exercise repository".should(
                () =>
                    {
                        var exerciseRepo = new ExerciseRepository(mongoConnectionString, "OevelsesPlan");
                        Exercise exercise = null, retrievedExercise = null, exerciseAfterUpdate;

                        "support all the CRUD operations on a single exercise".asIn(
                            () => exercise = exerciseRepo.Add(inputExercise)
                            ).andIn(
                                () =>
                                    {
                                        retrievedExercise = exerciseRepo.GetById(exercise.Id);

                                        retrievedExercise.Should().Equal(exercise);
                                        retrievedExercise.Name.Should().Equal(exercise.Name);
                                        retrievedExercise.MuscleGroup.Should().Equal(exercise.MuscleGroup);
                                        retrievedExercise.Muscle.Should().Equal(exercise.Muscle);
                                        retrievedExercise.Active.Should().Equal(exercise.Active);
                                        retrievedExercise.Description.Should().Equal(exercise.Description);
                                    }
                            ).andIn(
                                () =>
                                    {
                                        exercise.Name += "Test";
                                        exerciseRepo.Update(exercise);

                                        exerciseAfterUpdate = exerciseRepo.GetById(exercise.Id);

                                        exerciseAfterUpdate.Should().Equal(exercise);
                                        exerciseAfterUpdate.Name.Should().Not.Equal(retrievedExercise.Name);
                                    }
                            ).andIn(
                                () =>
                                    {
                                        exerciseRepo.Delete(exercise.Id);

                                        var deletedExercise = exerciseRepo.GetById(exercise.Id);

                                        deletedExercise.Should().Be.Null();
                                    });

                        "Be backed by a mongo db".asIn(
                            () =>
                                {
                                    var database = mongoServer.GetDatabase("OevelsesPlan");
                                    exercise = exerciseRepo.Add(inputExercise);

                                    var exercisesCollectionFromMongo = database.GetCollection<Exercise>("exercises");
                                    var exerciseRetrivedFromMonngo = exercisesCollectionFromMongo.Find(Query.EQ("_id", exercise.Id)).FirstOrDefault(); 

                                    exerciseRetrivedFromMonngo.Should().Not.Be.Null();
                                    exerciseRetrivedFromMonngo.Should().Equal(exercise);
                                });
                    });
        }

        [Theory, AutoData]
        public void given_an_exercise(Exercise sut)
        {
            var originalId = sut.Id;
            "Updating the exercise directly by column number".should(
                () =>
                    {
                        "update the name if the column number is 0".asIn(
                            () =>
                            {
                                sut.UpdatePropertyNumber(Exercise.PropertyNumbers.Name, "newName");
                                sut.Name.Should().Equal("newName");
                            });
                        "update the musclegroup if the column number is 1".asIn(
                            () =>
                            {
                                sut.UpdatePropertyNumber(Exercise.PropertyNumbers.MuscleGroup, "newMuscleGroup");
                                sut.MuscleGroup.Should().Equal("newMuscleGroup");
                            });
                        "update the muscle if the column number is 2".asIn(
                            () =>
                            {
                                sut.UpdatePropertyNumber(Exercise.PropertyNumbers.Muscle, "newMuscle");
                                sut.Muscle.Should().Equal("newMuscle");
                            });
                        "update the active property if the column number is 3".asIn(
                            () =>
                            {
                                sut.UpdatePropertyNumber(Exercise.PropertyNumbers.ActiveText, "Nej");
                                sut.Active.Should().Be.False();
                                sut.ActiveText.Should().Equal("Nej");
                            }); 
                        "update the description if the column number is 4".asIn(
                            () =>
                            {
                                sut.UpdatePropertyNumber(Exercise.PropertyNumbers.Description, "newDescription");
                                sut.Description.Should().Equal("newDescription");
                            });
                        "while leaving the id the same".asIn(
                            () => sut.Id.Should().Equal(originalId)
                            );
                    });

            "Updating the exercise through the repository by column number".should(
                () =>
                    {
                        var exercises = new ExerciseRepository(mongoConnectionString, "OevelsesPlan");
                        exercises.Add(sut);
                        "update the name if the column number is 0".asIn(
                            () =>
                                {
                                    exercises.UpdateByExerciseIdByPropertyNumber(sut.DT_RowId,
                                                                                 Exercise.PropertyNumbers.Name,
                                                                                 "newnewName");
                                    exercises.GetById(sut.Id).Name.Should().Equal("newnewName");
                                });
                        "update the musclegroup if the column number is 1".asIn(
                            () =>
                                {
                                    exercises.UpdateByExerciseIdByPropertyNumber(sut.DT_RowId,
                                                                                 Exercise.PropertyNumbers.MuscleGroup,
                                                                                 "newnewMuscleGroup");
                                    exercises.GetById(sut.Id).MuscleGroup.Should().Equal("newnewMuscleGroup");
                                });
                        "update the muscle if the column number is 2".asIn(
                            () =>
                                {
                                    exercises.UpdateByExerciseIdByPropertyNumber(sut.DT_RowId,
                                                                                 Exercise.PropertyNumbers.Muscle,
                                                                                 "newnewMuscle");
                                    exercises.GetById(sut.Id).Muscle.Should().Equal("newnewMuscle");
                                });
                        "update the active property if the column number is 3".asIn(
                            () =>
                                {
                                    exercises.UpdateByExerciseIdByPropertyNumber(sut.DT_RowId,
                                                                                 Exercise.PropertyNumbers.ActiveText,
                                                                                 "Ja");
                                    exercises.GetById(sut.Id).Active.Should().Be.True();
                                    exercises.GetById(sut.Id).ActiveText.Should().Equal("Ja");
                                });
                        "update the description if the column number is 4".asIn(
                            () =>
                                {
                                    exercises.UpdateByExerciseIdByPropertyNumber(sut.DT_RowId,
                                                                                 Exercise.PropertyNumbers.Description,
                                                                                 "newnewDescription");
                                    exercises.GetById(sut.Id).Description.Should().Equal("newnewDescription");
                                });
                    });

            "The DT_RowId".should(
                () => "be a serialization of the Id".asIn(
                    () => sut.DT_RowId.Should().Equal(originalId.ToString())
                          ));
        }
   }
}
