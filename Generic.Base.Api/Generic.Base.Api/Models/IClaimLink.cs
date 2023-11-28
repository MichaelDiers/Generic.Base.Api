namespace Generic.Base.Api.Models
{
    using System.Security.Claims;

    /// <summary>
    ///     A link for an operation including its requirements.
    /// </summary>
    /// <seealso cref="ILink" />
    public interface IClaimLink : ILink
    {
        /// <summary>
        ///     Determines whether the given <paramref name="userClaims" /> satisfy the required claims for an operation.
        /// </summary>
        /// <param name="userClaims">The claims the user owns.</param>
        /// <returns>
        ///     <c>true</c> if the claims satisfy the requirements; otherwise, <c>false</c>.
        /// </returns>
        bool CanBeAccessed(IEnumerable<Claim> userClaims);
    }
}
