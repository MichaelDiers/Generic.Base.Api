namespace Generic.Base.Api.Jwt
{
    using System.Security.Claims;

    /// <summary>
    ///     A service for creating json web tokens.
    /// </summary>
    public interface IJwtTokenService
    {
        /// <summary>
        ///     Creates the json web token.
        /// </summary>
        /// <param name="id">The identifier of the user.</param>
        /// <param name="displayName">The display name of the user.</param>
        /// <param name="claims">The claims of the user.</param>
        /// <returns>An <see cref="IToken" /> that contains access and refresh token.</returns>
        IToken CreateToken(string id, string displayName, IEnumerable<Claim> claims);
    }
}
