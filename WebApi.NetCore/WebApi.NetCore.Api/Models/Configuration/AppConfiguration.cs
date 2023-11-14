namespace WebApi.NetCore.Api.Models.Configuration
{
    using WebApi.NetCore.Api.Contracts.Configuration;

    /// <inheritdoc cref="IAppConfiguration" />
    public class AppConfiguration : IAppConfiguration
    {
        /// <summary>
        ///     Gets or sets the env configuration.
        /// </summary>
        public EnvNameConfiguration EnvNames { get; set; } = new();

        /// <summary>
        ///     Gets or sets the jwt configuration.
        /// </summary>
        public JwtConfiguration Jwt { get; set; } = new();

        /// <summary>
        ///     Gets or sets the env configuration.
        /// </summary>
        IEnvNameConfiguration IAppConfiguration.EnvNames => this.EnvNames;

        /// <summary>
        ///     Gets or sets the jwt configuration.
        /// </summary>
        IJwtConfiguration IAppConfiguration.Jwt => this.Jwt;
    }
}
