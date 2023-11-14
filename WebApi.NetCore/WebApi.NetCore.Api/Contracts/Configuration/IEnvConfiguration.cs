namespace WebApi.NetCore.Api.Contracts.Configuration
{
    /// <summary>
    ///     Access to the environment configuration. Values are set for google cloud run from the secret manager.
    /// </summary>
    public interface IEnvConfiguration
    {
        /// <summary>
        ///     Gets the API key.
        /// </summary>
        string ApiKey { get; }

        /// <summary>
        ///     Gets the JWT key.
        /// </summary>
        string JwtKey { get; }
    }
}
