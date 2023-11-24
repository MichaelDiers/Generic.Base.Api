namespace Generic.Base.Api.Tests.EnvironmentService
{
    using Generic.Base.Api.EnvironmentService;
    using Generic.Base.Api.Tests.Lib;

    /// <summary>
    ///     Tests for <see cref="IEnvironmentService" />.
    /// </summary>
    [Trait(
        "TestType",
        "InMemoryIntegrationTest")]
    public class EnvironmentServiceTests
    {
        [Theory]
        [InlineData(
            "ENV_SERVICE_TEST_VAR",
            "the value")]
        public void GetDirect(string key, string value)
        {
            Environment.SetEnvironmentVariable(
                key,
                value);

            var service = EnvironmentServiceDependencies.GetEnvironmentService();

            var actual = service.Get(key);

            Assert.Equal(
                value,
                actual);
        }

        [Theory]
        [InlineData(
            "ENV_SERVICE_TEST_VAR",
            "the value")]
        public void GetInjected(string key, string value)
        {
            Environment.SetEnvironmentVariable(
                key,
                value);

            var service =
                TestHostApplicationBuilder.GetService<IEnvironmentService>(
                    EnvironmentServiceDependencies.AddEnvironmentService);

            var actual = service.Get(key);

            Assert.Equal(
                value,
                actual);
        }
    }
}
