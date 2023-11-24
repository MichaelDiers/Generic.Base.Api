namespace WebApi.NetCore.Api.Contracts.Services
{
    using Generic.Base.Api.Jwt;

    /// <summary>
    ///     The auth business logic.
    /// </summary>
    public interface IAuthService
    {
        IToken SignIn(string id, params Role[] roles);
    }
}
