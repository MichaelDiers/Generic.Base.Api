namespace WebApi.NetCore.Api.Models.Configuration
{
    using WebApi.NetCore.Api.Contracts.Configuration;

    /// <inheritdoc cref="IEnvNameConfiguration" />
    public class EnvNameConfiguration : IEnvNameConfiguration
    {
        /// <summary>
        ///     Gets or sets the API key.
        /// </summary>
        public string ApiKeyName { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the JWT key.
        /// </summary>
        public string JwtKeyName { get; set; } = string.Empty;
    }
}
