namespace Generic.Base.Api.Models
{
    using Generic.Base.Api.Result;

    /// <inheritdoc cref="ILink" />
    public class Link : ILink
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Link" /> class.
        /// </summary>
        /// <param name="urn">The urn that specifies the operation.</param>
        /// <param name="url">The url of the operation.</param>
        public Link(Urn urn, string url)
        {
            this.Urn = $"urn:{urn.ToString()}";
            this.Url = url;
        }

        /// <summary>
        ///     Gets the url of the operation.
        /// </summary>
        public string Url { get; }

        /// <summary>
        ///     Gets the type of the operation.
        /// </summary>
        /// <seealso cref="Result.Urn" />
        public string Urn { get; }
    }
}
