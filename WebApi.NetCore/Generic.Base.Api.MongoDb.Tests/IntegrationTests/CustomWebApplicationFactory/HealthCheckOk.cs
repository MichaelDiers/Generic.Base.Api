namespace Generic.Base.Api.MongoDb.Tests.IntegrationTests.CustomWebApplicationFactory
{
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    internal class HealthCheckOk : IHealthCheck
    {
        /// <summary>
        ///     Runs the health check, returning the status of the component being checked.
        /// </summary>
        /// <param name="context">A context object associated with the current execution.</param>
        /// <param name="cancellationToken">
        ///     A <see cref="T:System.Threading.CancellationToken" /> that can be used to cancel the
        ///     health check.
        /// </param>
        /// <returns>
        ///     A <see cref="T:System.Threading.Tasks.Task`1" /> that completes when the health check has finished, yielding
        ///     the status of the component being checked.
        /// </returns>
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = new()
        )
        {
            return Task.FromResult(HealthCheckResult.Healthy(nameof(HealthCheckOk)));
        }
    }
}
