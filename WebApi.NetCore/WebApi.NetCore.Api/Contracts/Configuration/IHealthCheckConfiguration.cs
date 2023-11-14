namespace WebApi.NetCore.Api.Contracts.Configuration
{
    /// <summary>
    ///     The configuration of the health checks.
    /// </summary>
    public interface IHealthCheckConfiguration
    {
        /// <summary>
        ///     Gets the description of the documentation health check.
        /// </summary>
        string DocumentationDescription { get; }

        /// <summary>
        ///     Gets the documentation URL.
        /// </summary>
        string DocumentationUrl { get; }
    }
}
