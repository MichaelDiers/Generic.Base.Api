namespace Generic.Base.Api.Tests.EnvironmentService
{
    using Generic.Base.Api.EnvironmentService;
    using Generic.Base.Api.Tests.Lib;

    /// <summary>
    ///     Tests for <see cref="EnvironmentServiceDependencies" />.
    /// </summary>
    public class EnvironmentServiceDependenciesTests
    {
        [Fact]
        public void AddEnvironmentServiceOnceShouldAddOneService()
        {
            var services = TestHostApplicationBuilder
                .GetServices<IEnvironmentService>(EnvironmentServiceDependencies.AddEnvironmentService)
                .ToArray();

            Assert.Single(services);
            Assert.IsAssignableFrom<IEnvironmentService>(services.Single());
        }

        [Fact]
        public void AddEnvironmentServiceTwiceShouldAddOneService()
        {
            var services = TestHostApplicationBuilder.GetServices<IEnvironmentService>(
                    EnvironmentServiceDependencies.AddEnvironmentService,
                    EnvironmentServiceDependencies.AddEnvironmentService)
                .ToArray();

            Assert.Single(services);
            Assert.IsAssignableFrom<IEnvironmentService>(services.Single());
        }
    }
}
