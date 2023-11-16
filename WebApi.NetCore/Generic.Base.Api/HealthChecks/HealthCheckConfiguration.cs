namespace Generic.Base.Api.HealthChecks
{
    /// <inheritdoc cref="IHealthCheckConfiguration" />
    internal class HealthCheckConfiguration : IHealthCheckConfiguration
    {
        /// <summary>
        ///     The configuration section name in appSettings.json file.
        /// </summary>
        public static string ConfigurationSection = "HealthCheck";

        /// <summary>
        ///     Initializes a new instance of the <see cref="HealthCheckConfiguration" /> class.
        /// </summary>
        /// <param name="route">The route of the health check.</param>
        public HealthCheckConfiguration(string route)
        {
            this.Route = route;
        }

        /// <summary>
        ///     Gets the route. Omit leading slashes.
        /// </summary>
        public string Route { get; }
    }
}
