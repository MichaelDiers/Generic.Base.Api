namespace WebApi.NetCore.Api.HealthChecks
{
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using WebApi.NetCore.Api.Contracts.Configuration;

    /// <summary>
    ///     A health check for the online documentation.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck" />
    public class DocumentationHealthCheck : IHealthCheck
    {
        /// <summary>
        ///     The configuration of the health check.
        /// </summary>
        private readonly IHealthCheckConfiguration configuration;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DocumentationHealthCheck" /> class.
        /// </summary>
        /// <param name="configuration">The configuration of the health check.</param>
        public DocumentationHealthCheck(IHealthCheckConfiguration configuration)
        {
            this.configuration = configuration;
        }

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
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = new()
        )
        {
            using var client = new HttpClient();
            try
            {
                var response = await client.GetAsync(
                    this.configuration.DocumentationUrl,
                    cancellationToken);
                response.EnsureSuccessStatusCode();
                return HealthCheckResult.Healthy(this.configuration.DocumentationDescription);
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(
                    this.configuration.DocumentationDescription,
                    ex);
            }
        }
    }
}
