using Should.Fluent;
using Xunit;
using Specs;
using ØvelsesPlan.Model;

namespace ØvelsesPlanTests
{
    public class ExerciseSpec
    {
        [Fact]
        public void CRUDTest()
        {
            "The exercise data store".should(
                () =>
                    {
                        var exerciseRepo = new ExerciseRepository();
                        Exercise exercise = null, retrievedExercise = null, exerciseAfterUpdate;

                        "Support all the CRUD operations on a single exercise".asIn(
                            () => exercise = exerciseRepo.Add(
                                new Exercise(name: "Hip", muscleGroup: "Arm", muscle: "Bicpes", active:true, description: "Blah. 2 repetitioner" ))
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
                                }).andIn(
                                () =>
                                    {
                                        exerciseRepo.Delete(exercise.Id);

                                        var deletedExercise = exerciseRepo.GetById(exercise.Id);

                                        deletedExercise.Should().Be.Null();
                                    });

                    });
        }

        [Fact]
        public void GivenAnExercise()
        {
            var sut = new Exercise(name: "Foo", muscleGroup: "Bar", muscle: "Baz", active: true, description: "Blah");
            var originalId = sut.Id;
            "Updating to exercise by column number".should(
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
                        "update the musclegroup if the column number is 3".asIn(
                            () =>
                            {
                                sut.UpdatePropertyNumber(Exercise.PropertyNumbers.ActiveText, false);
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

            "The DT_RowId".should(
                () => "be a serialization of the Id".asIn(
                    () => sut.DT_RowId.Should().Equal(originalId.ToString())
                          ));
        }
   }
}
