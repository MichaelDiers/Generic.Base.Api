namespace Generic.Base.Api.Middleware.ApiKey
{
    using Generic.Base.Api.EnvironmentService;
    using Generic.Base.Api.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    /// <summary>
    ///     Api key dependencies.
    /// </summary>
    public static class ApiKeyDependencies
    {
        /// <summary>
        ///     Adds the api key configuration.
        /// </summary>
        /// <param name="webApplicationBuilder">The web application builder.</param>
        /// <returns>The given <paramref name="webApplicationBuilder" />.</returns>
        public static WebApplicationBuilder AddApiKey(this WebApplicationBuilder webApplicationBuilder)
        {
            var configuration =
                webApplicationBuilder.ReadFromConfiguration<ApiKeyConfiguration>(
                    ApiKeyConfiguration.ConfigurationSection);

            webApplicationBuilder.Services.AddEnvironmentService();
            var apiKey = EnvironmentService.GetValue(configuration.KeyName);
            configuration.ApiKey = apiKey;

            webApplicationBuilder.Services.TryAddSingleton<IApiKeyConfiguration>(_ => configuration);

            return webApplicationBuilder;
        }

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
