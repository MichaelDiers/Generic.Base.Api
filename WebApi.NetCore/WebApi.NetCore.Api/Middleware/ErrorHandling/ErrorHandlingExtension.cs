namespace WebApi.NetCore.Api.Middleware.ErrorHandling
{
    /// <summary>
    ///     Error handling extensions.
    /// </summary>
    public static class ErrorHandlingExtension
    {
        /// <summary>
        ///     Adds the error handling middleware.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns>An <see cref="IApplicationBuilder" />.</returns>
        public static IApplicationBuilder UseErrorHandling(this WebApplication app)
        {
            return app.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
