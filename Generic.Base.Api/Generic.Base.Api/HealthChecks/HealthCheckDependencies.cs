﻿namespace Generic.Base.Api.HealthChecks
{
    using Generic.Base.Api.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Routing;

    /// <summary>
    ///     Add health checks.
    /// </summary>
    public static class HealthCheckDependencies
    {
        /// <summary>
        ///     Maps the custom health checks.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns>
        ///     The result of
        ///     <see
        ///         cref="MapCustomHealthChecks(WebApplication,IHealthCheckConfiguration)" />
        /// </returns>
        public static IEndpointConventionBuilder MapCustomHealthChecks(this WebApplication app)
        {
            var configuration =
                app.ReadFromConfiguration<HealthCheckConfiguration>(HealthCheckConfiguration.ConfigurationSection);

            return app.MapCustomHealthChecks(configuration);
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
