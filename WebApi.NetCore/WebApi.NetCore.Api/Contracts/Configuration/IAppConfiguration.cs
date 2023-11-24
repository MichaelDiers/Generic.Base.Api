namespace WebApi.NetCore.Api.Contracts.Configuration
{
    using Generic.Base.Api.Jwt;

    /// <summary>
    ///     Describes the application configuration.
    /// </summary>
    public interface IAppConfiguration
    {
        /// <summary>
        ///     Gets the health check configuration.
        /// </summary>
        IDocumentationHealthCheckConfiguration DocumentationHealthCheck { get; }

        /// <summary>
        ///     Gets the env names used in <see cref="IEnvConfiguration" />.
        /// </summary>
        IEnvNameConfiguration EnvNames { get; }

        /// <summary>
        ///     Gets the jwt configuration.
        /// </summary>
        IJwtConfiguration Jwt { get; }
    }
}
