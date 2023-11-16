namespace Generic.Base.Api.Models
{
    using Generic.Base.Api.Configuration;

    /// <inheritdoc cref="IHealthCheckConfiguration" />
    public class HealthCheckConfiguration : IHealthCheckConfiguration
    {
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
