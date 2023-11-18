namespace Generic.Base.Api.Models
{
    /// <inheritdoc cref="ILinkResult" />
    public class LinkResult : ILinkResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LinkResult" /> class.
        /// </summary>
        /// <param name="links">The links that describes available operations.</param>
        public LinkResult(IEnumerable<ILink> links)
        {
            this.Links = links;
        }

        /// <summary>
        ///     Gets the links to available operations.
        /// </summary>
        public IEnumerable<ILink> Links { get; }
    }
}
