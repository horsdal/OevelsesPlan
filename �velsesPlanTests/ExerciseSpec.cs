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
                                        exerciseRepo.Delete(exercise);

                                        var deletedExercise = exerciseRepo.GetById(exercise.Id);

                                        deletedExercise.Should().Be.Null();
                                    });

                    });
        }
   }
}
