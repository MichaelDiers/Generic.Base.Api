namespace WebApi.NetCore.Api.HealthChecks
{
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;

    /// <summary>
    ///     Add health checks.
    /// </summary>
    public static class HealthCheckExtension
    {
        /// <summary>
        ///     Adds the health checks to the given <paramref name="services" />.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks().AddCheck<DocumentationHealthCheck>(nameof(DocumentationHealthCheck));

            return services;
        }

        /// <summary>
        ///     Maps the custom health checks.
        /// </summary>
        /// <param name="app">The application.</param>
        public static IEndpointConventionBuilder MapCustomHealthChecks(this WebApplication app)
        {
            return app.MapHealthChecks(
                "/health",
                new HealthCheckOptions {ResponseWriter = HealthCheckWriter.WriteHealthReport});
        }
    }
}
