using System.IO;
using Nancy;
using Nancy.Testing;
using Should.Fluent;
using Xunit;
using Specs;
using ØvelsesPlan;

namespace ØvelsesPlanTests
{
    public class WebApiSpec : MongoIntegrationSpec
    {
        private Browser app;

        public WebApiSpec()
        {
            ExercisePlannerModule artificialReference;
            var bootstrapper = new DefaultNancyBootstrapper();
            bootstrapper.Initialise();
            app = new Browser(bootstrapper);
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
                            () => Assert.True(true) //TDB  
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
    }
}
