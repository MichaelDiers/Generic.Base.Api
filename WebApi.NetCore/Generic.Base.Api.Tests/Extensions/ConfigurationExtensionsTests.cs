namespace Generic.Base.Api.Tests.Extensions
{
    using Generic.Base.Api.Extensions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    ///     Tests for <see cref="Api.Extensions.ConfigurationExtensions" />.
    /// </summary>
    [Trait(
        "TestType",
        "UnitTest")]
    public class ConfigurationExtensionsTests
    {
        [Fact]
        public void ReadFromConfigurationShouldFailIfSectionDoesNotExist()
        {
            var builder = new HostApplicationBuilder();

            Assert.Throws<ArgumentException>(
                () => builder.Configuration.ReadFromConfiguration<ExtensionTest>(nameof(ExtensionTest)));
        }

        [Fact]
        public void ReadFromConfigurationShouldSucceedIfSectionExists()
        {
            var builder = new HostApplicationBuilder();
            builder.Configuration.AddJsonFile("Extensions/test.json");

            var actual = builder.Configuration.ReadFromConfiguration<ExtensionTest>(nameof(ExtensionTest));
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
