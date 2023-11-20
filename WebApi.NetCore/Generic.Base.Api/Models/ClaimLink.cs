namespace Generic.Base.Api.Models
{
    using System.Security.Claims;
    using System.Text.RegularExpressions;
    using Generic.Base.Api.AuthServices.UserService;

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
        ///     Initializes a new instance of the <see cref="ClaimLink" /> class.
        /// </summary>
        /// <param name="urn">The urn that specifies the operation.</param>
        /// <param name="url">The url of the operation.</param>
        /// <param name="claims">The claims required for the operation.</param>
        private ClaimLink(string urn, string url, IEnumerable<Claim> claims)
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

        /// <summary>
        ///     Initializes a new instance of the <see cref="ClaimLink" /> class.
        /// </summary>
        /// <param name="urnNamespace">The namespace of the urn.</param>
        /// <param name="urn">The urn of the operation.</param>
        /// <param name="url">The url of the operation.</param>
        /// <returns>A new instance of <see cref="ClaimLink" />.</returns>
        public static ClaimLink Create(string urnNamespace, Urn urn, string url)
        {
            return ClaimLink.Create(
                urnNamespace,
                urn,
                url,
                Enumerable.Empty<Claim>());
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ClaimLink" /> class.
        /// </summary>
        /// <param name="urnNamespace">The namespace of the urn.</param>
        /// <param name="urn">The urn of the operation.</param>
        /// <param name="url">The url of the operation.</param>
        /// <param name="claim">A required claim for executing the operation.</param>
        /// <param name="claims">The claims required for executing the operation.</param>
        /// <returns>A new instance of <see cref="ClaimLink" /> as an <see cref="IClaimLink" />.</returns>
        public static IClaimLink Create(
            string urnNamespace,
            Urn urn,
            string url,
            Claim claim,
            params Claim[] claims
        )
        {
            return ClaimLink.Create(
                urnNamespace,
                urn,
                url,
                claims.Append(claim));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ClaimLink" /> class.
        /// </summary>
        /// <param name="urnNamespace">The namespace of the urn.</param>
        /// <param name="urn">The urn of the operation.</param>
        /// <param name="url">The url of the operation.</param>
        /// <param name="role">A role required for executing the operation.</param>
        /// <param name="roles">The roles required for executing the operation.</param>
        /// <returns>A new instance of <see cref="ClaimLink" />.</returns>
        public static ClaimLink Create(
            string urnNamespace,
            Urn urn,
            string url,
            Role role,
            params Role[] roles
        )
        {
            return ClaimLink.Create(
                urnNamespace,
                urn,
                url,
                roles.Append(role)
                .Select(
                    r => new Claim(
                        ClaimTypes.Role,
                        r.ToString())));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ClaimLink" /> class.
        /// </summary>
        /// <param name="urnNamespace">The namespace of the urn.</param>
        /// <param name="urn">The urn of the operation.</param>
        /// <param name="url">The url of the operation.</param>
        /// <param name="claims">The claims required for executing the operation.</param>
        /// <returns>A new instance of <see cref="ClaimLink" />.</returns>
        private static ClaimLink Create(
            string urnNamespace,
            Urn urn,
            string url,
            IEnumerable<Claim> claims
        )
        {
            return new ClaimLink(
                $"urn:{Regex.Replace(urnNamespace, "Controller(Base)?$", string.Empty)}:{urn.ToString()}",
                url,
                claims);
        }
    }
}
