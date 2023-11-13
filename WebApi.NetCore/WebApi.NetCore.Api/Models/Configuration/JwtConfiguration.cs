namespace WebApi.NetCore.Api.Models.Configuration
{
    using WebApi.NetCore.Api.Contracts.Configuration;

    /// <inheritdoc cref="IJwtConfiguration" />
    public class JwtConfiguration : IJwtConfiguration
    {
        /// <summary>
        ///     Gets or sets the audience.
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the issuer.
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the name of the key used by the secret manager.
        /// </summary>
        public string KeyName { get; set; } = string.Empty;
    }
}
