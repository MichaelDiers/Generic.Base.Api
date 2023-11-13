namespace WebApi.NetCore.Api.Services
{
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

        public string SignIn(params Role[] roles)
        {
            return this.jwtTokenService.CreateToken(roles);
        }
    }
}
