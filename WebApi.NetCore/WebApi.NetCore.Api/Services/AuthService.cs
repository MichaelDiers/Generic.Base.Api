namespace WebApi.NetCore.Api.Services
{
    using System.Security.Claims;
    using Generic.Base.Api.Jwt;
    using WebApi.NetCore.Api.Contracts;
    using WebApi.NetCore.Api.Contracts.Services;

    /// <inheritdoc cref="IAuthService" />
    /// <seealso cref="IAuthService" />
    internal class AuthService : IAuthService
    {
        private readonly IJwtTokenService jwtTokenService;

        public AuthService(IJwtTokenService jwtTokenService)
        {
            this.jwtTokenService = jwtTokenService;
        }

        public IToken SignIn(string id, params Role[] roles)
        {
            var claims = roles.Select(
                role => new Claim(
                    ClaimTypes.Role,
                    role.ToString()));

            return this.jwtTokenService.CreateToken(
                id,
                id,
                claims);
        }
    }
}
