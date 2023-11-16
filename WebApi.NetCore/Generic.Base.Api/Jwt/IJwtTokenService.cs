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
        /// <param name="claims">The claims of the user.</param>
        /// <returns>The token as a string.</returns>
        string CreateToken(IEnumerable<Claim> claims);
    }
}
