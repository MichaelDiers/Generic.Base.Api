namespace Generic.Base.Api.Jwt
{
    using Generic.Base.Api.EnvironmentService;
    using Generic.Base.Api.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    ///     Extensions for <see cref="JwtTokenService" />.
    /// </summary>
    public static class JwtTokenServiceExtensions
    {
        /// <summary>
        ///     Adds the jwt service and configuration.
        /// </summary>
        /// <param name="webApplicationBuilder">The web application builder.</param>
        /// <returns>The given <paramref name="webApplicationBuilder" />.</returns>
        public static WebApplicationBuilder AddJwt(this WebApplicationBuilder webApplicationBuilder)
        {
            var configuration =
                webApplicationBuilder.ReadFromConfiguration<JwtConfiguration>(JwtConfiguration.ConfigurationSection);

            var environmentService = webApplicationBuilder.Services.AddEnvironmentService();
            var key = environmentService.Get(configuration.KeyName);
            configuration.Key = key;

            return webApplicationBuilder.AddJwt(configuration);
        }

        /// <summary>
        ///     Adds the jwt service and configuration.
        /// </summary>
        /// <param name="webApplicationBuilder">The web application builder.</param>
        /// <param name="jwtConfiguration">The jwt configuration.</param>
        /// <returns>The given <paramref name="webApplicationBuilder" />.</returns>
        public static WebApplicationBuilder AddJwt(
            this WebApplicationBuilder webApplicationBuilder,
            IJwtConfiguration jwtConfiguration
        )
        {
            webApplicationBuilder.Services.AddScoped<IJwtConfiguration>(_ => jwtConfiguration);
            webApplicationBuilder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            return webApplicationBuilder;
        }
    }
}
