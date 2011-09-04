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
        public void when_call_base_url()
        {

            var response = app.Get("/", with => with.HttpRequest());

            "the app".should
                (() =>
                    {
                        "return the index page".asIn
                            (
                                () => response.StatusCode.Should().Equal(HttpStatusCode.OK)
                            ).andIn(
                                () => response.Body["title"].ShouldContain("Øvelsesplan")
                            );
                        "have retrieved the current week plan".asIn
                            (
                                () => Assert.True(true) //TDB  
                            );
                    }
                );
        }
    }
}
