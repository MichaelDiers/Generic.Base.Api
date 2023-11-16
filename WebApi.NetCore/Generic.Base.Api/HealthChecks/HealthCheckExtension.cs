namespace Generic.Base.Api.HealthChecks
{
    using Generic.Base.Api.Configuration;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;

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
        public static IHealthChecksBuilder AddCustomHealthChecks(this IServiceCollection services)
        {
            return services.AddHealthChecks();
        }

        /// <summary>
        ///     Maps the custom health checks.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="configuration">The configuration of the health check.</param>
        /// <returns>
        ///     The result of
        ///     <see
        ///         cref="HealthCheckEndpointRouteBuilderExtensions.MapHealthChecks(IEndpointRouteBuilder,string,HealthCheckOptions)" />
        /// </returns>
        public static IEndpointConventionBuilder MapCustomHealthChecks(
            this WebApplication app,
            IHealthCheckConfiguration configuration
        )
        {
            return app.MapHealthChecks(
                $"/{configuration.Route}",
                new HealthCheckOptions {ResponseWriter = HealthCheckWriter.WriteHealthReport});
        }
    }
}
