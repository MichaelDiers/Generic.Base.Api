namespace WebApi.NetCore.Api.Contracts.Configuration
{
    /// <summary>
    ///     Describes the application configuration.
    /// </summary>
    public interface IAppConfiguration
    {
        /// <summary>
        ///     Gets the jwt configuration.
        /// </summary>
        IJwtConfiguration Jwt { get; }
    }
}
