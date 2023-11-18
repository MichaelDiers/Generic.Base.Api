namespace Generic.Base.Api.Tests.Extensions
{
    using Generic.Base.Api.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    ///     Tests for <see cref="WebApplicationExtensions" />.
    /// </summary>
    public class WebApplicationExtensionsTests
    {
        [Fact]
        public void ReadFromConfigurationShouldFailIfSectionDoesNotExist()
        {
            var app = WebApplication.CreateBuilder().Build();

            Assert.Throws<ArgumentException>(() => app.ReadFromConfiguration<ExtensionTest>(nameof(ExtensionTest)));
        }

        [Fact]
        public void ReadFromConfigurationShouldSucceedIfSectionExists()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Configuration.AddJsonFile("Extensions/test.json");
            var app = builder.Build();

            var actual = app.ReadFromConfiguration<ExtensionTest>(nameof(ExtensionTest));
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
