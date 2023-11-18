namespace Generic.Base.Api.Tests.HashService
{
    using Generic.Base.Api.HashService;
    using Generic.Base.Api.Tests.Lib;

    /// <summary>
    ///     Tests for <see cref="IHashService" />
    /// </summary>
    public class HashServiceTests
    {
        [Theory]
        [InlineData("password")]
        public void Hash(string password)
        {
            var service = TestHostApplicationBuilder.GetService<IHashService>(HashServiceDependencies.AddHashService);

            var hashed = service.Hash(password);
            Assert.NotEqual(
                password,
                hashed);
        }

        [Theory]
        [InlineData("password")]
        public void Verify(string password)
        {
            var service = TestHostApplicationBuilder.GetService<IHashService>(HashServiceDependencies.AddHashService);

            var hashed = service.Hash(password);

            Assert.True(
                service.Verify(
                    password,
                    hashed));
        }
    }
}
