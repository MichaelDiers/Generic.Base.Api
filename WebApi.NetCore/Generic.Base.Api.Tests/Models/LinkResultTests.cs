namespace Generic.Base.Api.Tests.Models
{
    using Generic.Base.Api.Models;

    /// <summary>
    ///     Tests for <see cref="LinkResult" />.
    /// </summary>
    public class LinkResultTests
    {
        [Fact]
        public void Ctor()
        {
            var links = new[]
            {
                new Link(
                    Urn.Delete,
                    "url"),
                new Link(
                    Urn.Create,
                    "url other")
            };

            var linkResult = new LinkResult(links);

            Assert.Equal(
                links.Length,
                linkResult.Links.Count());
            foreach (var link in links)
            {
                Assert.Contains(
                    linkResult.Links,
                    l => l.Url == link.Url && l.Urn == link.Urn);
            }
        }

        [Fact]
        public void IsILinkResult()
        {
            Assert.IsAssignableFrom<ILinkResult>(new LinkResult(Enumerable.Empty<Link>()));
        }
    }
}
