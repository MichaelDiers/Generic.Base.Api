namespace Generic.Base.Api.HealthChecks
{
    /// <summary>
    ///     The configuration of the health checks.
    /// </summary>
    public interface IHealthCheckConfiguration
    {
        /// <summary>
        ///     Gets the route. Omit leading slashes.
        /// </summary>
        string Route { get; }
    }
}
