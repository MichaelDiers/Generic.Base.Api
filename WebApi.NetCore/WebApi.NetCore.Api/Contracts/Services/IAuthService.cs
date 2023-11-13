namespace WebApi.NetCore.Api.Contracts.Services
{
    /// <summary>
    ///     The auth business logic.
    /// </summary>
    public interface IAuthService
    {
        string SignIn(params Role[] roles);
    }
}
