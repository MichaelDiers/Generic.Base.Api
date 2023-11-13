namespace WebApi.NetCore.Api.Contracts.Services
{
    internal interface IJwtTokenService
    {
        /// <summary>
        ///     Creates the token.
        /// </summary>
        /// <param name="roles">The roles of the user.</param>
        /// <returns>The token as a string.</returns>
        string CreateToken(IEnumerable<Role> roles);
    }
}
