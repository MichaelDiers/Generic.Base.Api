namespace Generic.Base.Api.Tests.HashService
{
    using Generic.Base.Api.HashService;
    using Generic.Base.Api.Tests.Lib;

    /// <summary>
    ///     Test for <see cref="HashServiceDependencies.AddHashService" />.
    /// </summary>
    public class HashServiceDependenciesTests
    {
        [Fact]
        public void AddHashServiceOnceShouldAddOneService()
        {
            var services = TestHostApplicationBuilder.GetServices<IHashService>(HashServiceDependencies.AddHashService)
                .ToArray();

            Assert.Single(services);
            Assert.IsAssignableFrom<IHashService>(services.Single());
        }

        [Fact]
        public void AddHashServiceTwiceShouldAddOneService()
        {
            var services = TestHostApplicationBuilder.GetServices<IHashService>(
                    HashServiceDependencies.AddHashService,
                    HashServiceDependencies.AddHashService)
                .ToArray();

            Assert.Single(services);
            Assert.IsAssignableFrom<IHashService>(services.Single());
        }
    }
}
