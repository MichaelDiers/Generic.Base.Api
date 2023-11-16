﻿namespace Generic.Base.Api.Middleware.ApiKey
{
    using Microsoft.AspNetCore.Builder;

    /// <summary>
    ///     Api key extensions.
    /// </summary>
    public static class ApiKeyExtension
    {
        /// <summary>
        ///     Adds the api key handling middleware.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns>An <see cref="IApplicationBuilder" />.</returns>
        public static IApplicationBuilder UseApiKey(this WebApplication app)
        {
            return app.UseMiddleware<ApiKeyMiddleware>();
        }
    }
}
