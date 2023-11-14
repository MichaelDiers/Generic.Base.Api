namespace WebApi.NetCore.Api.Models.Configuration
{
    using WebApi.NetCore.Api.Contracts.Configuration;

    /// <inheritdoc cref="IEnvConfiguration" />
    public class EnvConfiguration : IEnvConfiguration
    {
        /// <summary>
        ///     Gets the API key.
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        ///     Gets the JWT key.
        /// </summary>
        public string JwtKey { get; set; } = string.Empty;
    }
}
