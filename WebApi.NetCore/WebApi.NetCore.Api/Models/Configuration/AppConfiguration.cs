namespace WebApi.NetCore.Api.Models.Configuration
{
    using Generic.Base.Api.Jwt;
    using WebApi.NetCore.Api.Contracts.Configuration;

    /// <inheritdoc cref="IAppConfiguration" />
    public class AppConfiguration : IAppConfiguration
    {
        /// <summary>
        ///     Gets or sets the health check configuration.
        /// </summary>
        public DocumentationHealthCheckConfiguration DocumentationHealthCheck { get; set; } = new();

        /// <summary>
        ///     Gets or sets the env configuration.
        /// </summary>
        public EnvNameConfiguration EnvNames { get; set; } = new();

        /// <summary>
        ///     Gets or sets the jwt configuration.
        /// </summary>
        public JwtConfiguration Jwt { get; set; }

        /// <summary>
        ///     Gets the health check configuration.
        /// </summary>
        IDocumentationHealthCheckConfiguration IAppConfiguration.DocumentationHealthCheck =>
            this.DocumentationHealthCheck;

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
