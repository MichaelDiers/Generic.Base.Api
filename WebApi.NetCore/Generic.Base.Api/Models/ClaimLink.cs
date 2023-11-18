namespace Generic.Base.Api.Models
{
    using System.Security.Claims;

    /// <summary>
    ///     A link for an operation including its requirements.
    /// </summary>
    /// <seealso cref="Link" />
    public class ClaimLink : Link, IClaimLink
    {
        /// <summary>
        ///     The required claims of an operation.
        /// </summary>
        private readonly IEnumerable<Claim> claims;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ClaimLink" /> class.
        /// </summary>
        /// <param name="urn">The urn that specifies the operation.</param>
        /// <param name="url">The url of the operation.</param>
        public ClaimLink(Urn urn, string url)
            : this(
                urn,
                url,
                Enumerable.Empty<Claim>())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ClaimLink" /> class.
        /// </summary>
        /// <param name="urn">The urn that specifies the operation.</param>
        /// <param name="url">The url of the operation.</param>
        /// <param name="claims">A list of <see cref="Claim" /> that is required for executing the operation.</param>
        public ClaimLink(Urn urn, string url, IEnumerable<Claim> claims)
            : base(
                urn,
                url)
        {
            this.claims = claims;
        }

        /// <summary>
        ///     Determines whether the given <paramref name="userClaims" /> satisfy the required claims for an operation.
        /// </summary>
        /// <param name="userClaims">The claims the user owns.</param>
        /// <returns>
        ///     <c>true</c> if the claims satisfy the requirements; otherwise, <c>false</c>.
        /// </returns>
        public bool CanBeAccessed(IEnumerable<Claim> userClaims)
        {
            return this.claims.All(
                claim => userClaims.Any(userClaim => userClaim.Type == claim.Type && userClaim.Value == claim.Value));
        }
    }
}
