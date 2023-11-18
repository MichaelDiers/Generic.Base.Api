namespace Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory
{
    using Generic.Base.Api.Models;

    /// <summary>
    ///     A deserializable version of a link result.
    /// </summary>
    internal class ClientLinkResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ClientLinkResult" /> class.
        /// </summary>
        /// <param name="links">The links.</param>
        public ClientLinkResult(IEnumerable<Link> links)
        {
            this.Links = links;
        }

        /// <summary>
        ///     Gets the links to available operations.
        /// </summary>
        public IEnumerable<Link> Links { get; }
    }
}
