using Nancy;
using Nancy.Testing;
using Should.Fluent;
using Xunit;
using Specs;
using ØvelsesPlan;
using ØvelsesPlan.DataAccess;
using ØvelsesPlan.Model;

namespace ØvelsesPlanTests
{
    public class WebApiSpec : MongoIntegrationSpec
    {
        private Browser app;

        public WebApiSpec()
        {
            ExercisePlannerModule artificialReference;
            var bootstrapper = new Bootstrapper();
            bootstrapper.Initialise();
            app = new Browser(bootstrapper);

            var exercises = new MongoExerciseRepository(mongoConnectionString);
            for (int i = 0; i < 10; i++)
                exercises.Add(new Exercise(name: "Hop" + i, muscleGroup: "Lår", muscle: "Quadrozeps pemoris", active: true, description: "Foo"));
        }

        [Fact]
        public void when_calling_base_url()
        {
            var response = app.Get("/", with => with.HttpRequest());

            "the app".should
                (() =>
                 "return the index page".asIn
                     (
                         () => response.StatusCode.Should().Equal(HttpStatusCode.OK)
                     ).andIn(
                         () => response.Body["title"].ShouldContain("Øvelsesplan"))
                );
        }

        [Fact]
        public void when_calling_exercises()
        {
            var response = app.Get("/exercises/", with => with.HttpRequest());

            "the app".should
                (() =>
                 "return the exercises page".asIn
                     (
                         () => response.StatusCode.Should().Equal(HttpStatusCode.OK)
                     ).andIn(
                         () => response.Body["title"].ShouldContain("Øvelser"))
                );
        }

        [Fact]
        public void when_calling_exercises_all()
        {
            var response = app.Get("/exercises/all/", with => with.HttpRequest());

            "the app".should
                (() =>
                     {
                         "return json".asIn
                             (
                                 () => response.StatusCode.Should().Equal(HttpStatusCode.OK)
                             ).andIn(
                                 () => response.Context.Response.ContentType.Should().Contain("json"));

                         "have retrieved all exercises".asIn
                             (
                                 () =>
                                     {
                                         var jsonResponse = response.GetBodyAsString();
                                         jsonResponse.Should().Contain("aaData");
                                         var jsonArray = jsonResponse.Substring(jsonResponse.IndexOf("aaData"));
                                         for (int i = 0; i < 10; i++)
                                             jsonArray.Should().Contain("Hop" + i);
                                     }
                             );
                     });
        }

        [Fact]
        public void when_calling_weekplan_current()
        {
            var response = app.Get("/weekplan/current/", with => with.HttpRequest());

            "the app".should
                (() =>
                     {
                         "return json".asIn
                             (
                                 () => response.StatusCode.Should().Equal(HttpStatusCode.OK)
                             ).andIn(
                                 () => response.Context.Response.ContentType.Should().Contain("json"));

                         "have retrieved the current weekplan".asIn
                             (
                                 () => Assert.True(true) //TDB  
                             );
                     });
        }

        [Fact]
        public void when_calling_weekplan_weeknumber()
        {
            var response = app.Get("/weekplan/30/", with => with.HttpRequest());

            "the app".should
                (() =>
                     {
                         "return json".asIn
                             (
                                 () => response.StatusCode.Should().Equal(HttpStatusCode.OK)
                             ).andIn(
                                 () => response.Context.Response.ContentType.Should().Contain("json"));

                         "have retrieved the weekplan for week 30".asIn
                             (
                                 () => Assert.True(true) //TDB  
                             );
                     });
        }

        [Fact]
        public void when_posting_to_exercises_create()
        {
            var response = app.Post("/exercises/create", with => with.HttpRequest());

            "the app".should
                (() =>
                     {
                         "return json.".asIn
                             (
                                 () => response.StatusCode.Should().Equal(HttpStatusCode.OK)
                             ).andIn(
                                 () => response.Context.Response.ContentType.Should().Contain("json"));

                         "create a new exercise".asIn
                             (
                                 () =>
                                     {
                                         var jsonResponse = response.GetBodyAsString();
                                         jsonResponse.Should().Contain("Name");
                                         jsonResponse.Should().Contain("MuscleGroup");
                                         jsonResponse.Should().Contain("Muscle");
                                         jsonResponse.Should().Contain("Active");
                                         jsonResponse.Should().Contain("Description");
                                     }
                             );
                     });
        }

        [Fact]
        public void when_posting_to_exercises_delete()
        {
            var response = app.Post("/exercises/delete", with =>
                                                             {
                                                                 with.HttpRequest();
                                                                 with.FormValue("row_id", "42");
                                                             });

            "the app".should
                (() =>
                     {
                         "return ok.".asIn
                             (
                                 () => response.StatusCode.Should().Equal(HttpStatusCode.OK)
                             );

                         "delete exercise 42".asIn
                             (
                                 () => Assert.True(true) //TDB  
                             );
                     });
        }

        [Fact]
        public void when_posting_to_exercises_edit()
        {
            var response = app.Post("/exercises/edit", with =>
            {
                with.HttpRequest();
                with.FormValue("row_id", "42");
                with.FormValue("column", "2");
                with.FormValue("value", "ny værdi");
            });

            "the app".should
                (() =>
                     {
                         "return the new value in a string.".asIn
                             (
                                 () => response.StatusCode.Should().Equal(HttpStatusCode.OK)
                             ).andIn
                             (
                                 () => response.Context.Response.ContentType.Should().Contain("text/html")
                             );

                         "edit exercise 42".asIn
                             (
                                 () => Assert.True(true) //TDB  
                             );
                     });
        }

        [Fact]
        public void when_posting_to_weekplan_create()
        {
            var response = app.Post("/weekplan/create", with =>
            {
                with.HttpRequest();
                with.FormValue("weeknumber", "30");
            });

            "the app".should
                (() =>
                {
                    "return the new plan as json.".asIn
                        (
                            () => response.StatusCode.Should().Equal(HttpStatusCode.OK)
                        ).andIn
                        (
                            () => response.Context.Response.ContentType.Should().Contain("application/json")
                        );

                    "return the weeknumber 30 within the json response".asIn
                        (
                            () => Assert.True(true) //TDB  
                        );

                    "create a new plan for week 30".asIn
                        (
                            () => Assert.True(true) //TDB  
                        );
                });
        }
    }
}
