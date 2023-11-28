namespace Generic.Base.Api.Tests.Extensions
{
    using Generic.Base.Api.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    ///     Tests for <see cref="WebApplicationBuilderExtensions" />.
    /// </summary>
    [Trait(
        "TestType",
        "UnitTest")]
    public class WebApplicationBuilderExtensionsTests
    {
        [Fact]
        public void ReadFromConfigurationShouldFailIfSectionDoesNotExist()
        {
            var builder = WebApplication.CreateBuilder();

            Assert.Throws<ArgumentException>(() => builder.ReadFromConfiguration<ExtensionTest>(nameof(ExtensionTest)));
        }

        [Fact]
        public void ReadFromConfigurationShouldSucceedIfSectionExists()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Configuration.AddJsonFile("Extensions/test.json");

            var actual = builder.ReadFromConfiguration<ExtensionTest>(nameof(ExtensionTest));
            Assert.NotNull(actual);

            Assert.Equal(
                $"{nameof(ExtensionTest)}Value1",
                actual.ExtensionTestKey1);
            Assert.Equal(
                $"{nameof(ExtensionTest)}Value2",
                actual.ExtensionTestKey2);
        }
    }
}
