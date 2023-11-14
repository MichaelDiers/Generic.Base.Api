namespace WebApi.NetCore.Api.Contracts.Configuration
{
    /// <summary>
    ///     Describes the jwt configuration.
    /// </summary>
    public interface IJwtConfiguration
    {
        /// <summary>
        ///     Gets the audience.
        /// </summary>
        string Audience { get; }

        /// <summary>
        ///     Gets the issuer.
        /// </summary>
        string Issuer { get; }
    }
}
