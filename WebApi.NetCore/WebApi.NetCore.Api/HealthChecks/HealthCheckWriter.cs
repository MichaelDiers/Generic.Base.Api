namespace WebApi.NetCore.Api.HealthChecks
{
    using System.Text.Json;
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    /// <summary>
    ///     Provides a simple health check result writer.
    /// </summary>
    public static class HealthCheckWriter
    {
        /// <summary>
        ///     Writes the health report response.
        /// </summary>
        /// <param name="context">The current http context.</param>
        /// <param name="healthReport">The health report.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public static Task WriteHealthReport(HttpContext context, HealthReport healthReport)
        {
            var report = new
            {
                status = healthReport.Status.ToString(),
                healthy =
                    $"{healthReport.Entries.Count(entry => entry.Value.Status == HealthStatus.Healthy)}/{healthReport.Entries.Values.Count()}",
                results = healthReport.Entries.Select(
                    entry => new
                    {
                        check = entry.Key,
                        status = entry.Value.Status.ToString(),
                        description = entry.Value.Description,
                        exception = entry.Value.Exception?.Message,
                        data = entry.Value.Data.Select(
                            dat => new
                            {
                                key = dat.Key,
                                value = dat.Value
                            })
                    })
            };

            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(JsonSerializer.Serialize(report));
        }
    }
}
