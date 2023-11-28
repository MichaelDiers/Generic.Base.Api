namespace Generic.Base.Api.AuthServices.TokenService
{
    using Generic.Base.Api.Database;
    using Generic.Base.Api.Models;

    /// <summary>
    ///     Describes a token entry that is sent to the client.
    /// </summary>
    /// <seealso cref="LinkResult" />
    public class ResultTokenEntry : LinkResult, IIdEntry
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ResultTokenEntry" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userId">The owner of the token.</param>
        /// <param name="links">The links that describes available operations.</param>
        /// <param name="validUntil">The information until when the token is valid.</param>
        public ResultTokenEntry(
            string id,
            string userId,
            string validUntil,
            IEnumerable<Link> links
        )
            : base(links)
        {
            this.Id = id;
            this.UserId = userId;
            this.ValidUntil = validUntil;
        }

        /// <summary>
        ///     Gets the owner of the token.
        /// </summary>

        public string UserId { get; }

        /// <summary>
        ///     Gets the information until when the token is valid.
        /// </summary>
        public string ValidUntil { get; }

        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        public string Id { get; }
    }
}
