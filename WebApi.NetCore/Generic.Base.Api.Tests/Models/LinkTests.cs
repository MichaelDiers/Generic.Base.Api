namespace Generic.Base.Api.Tests.Models
{
    using Generic.Base.Api.Models;

    /// <summary>
    ///     Tests for <see cref="Link" />.
    /// </summary>
    [Trait(
        "TestType",
        "UnitTest")]
    public class LinkTests
    {
        [Theory]
        [InlineData(
            "urn:" + nameof(Urn.Options),
            "url")]
        public void CtorWithString(string urn, string url)
        {
            var link = new Link(
                urn,
                url);

            Assert.Equal(
                urn,
                link.Urn);
            Assert.Equal(
                url,
                link.Url);
        }

        [Theory]
        [InlineData(
            Urn.ReadAll,
            "url")]
        public void CtorWithUrn(Urn urn, string url)
        {
            var link = new Link(
                urn,
                url);

            Assert.Equal(
                $"urn:{urn}",
                link.Urn);
            Assert.Equal(
                url,
                link.Url);
        }

        [Fact]
        public void IsILink()
        {
            Assert.IsAssignableFrom<ILink>(
                new Link(
                    Urn.Options,
                    "url"));
        }
    }
}
