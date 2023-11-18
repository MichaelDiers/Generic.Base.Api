namespace Generic.Base.Api.Jwt
{
    using System.Text;
    using Generic.Base.Api.EnvironmentService;
    using Generic.Base.Api.Extensions;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.IdentityModel.Tokens;

    /// <summary>
    ///     Extensions for <see cref="JwtTokenService" />.
    /// </summary>
    public static class JwtTokenServiceDependencies
    {
        /// <summary>
        ///     Adds the jwt service and configuration.
        /// </summary>
        /// <param name="webApplicationBuilder">The web application builder.</param>
        /// <returns>The given <paramref name="webApplicationBuilder" />.</returns>
        public static WebApplicationBuilder AddJwtTokenService(this WebApplicationBuilder webApplicationBuilder)
        {
            var configuration =
                webApplicationBuilder.ReadFromConfiguration<JwtConfiguration>(JwtConfiguration.ConfigurationSection);
            webApplicationBuilder.Services.AddEnvironmentService();

            var key = EnvironmentService.GetValue(configuration.KeyName);
            configuration.Key = key;

            return webApplicationBuilder.AddJwtTokenService(configuration);
        }

        /// <summary>
        ///     Adds the jwt service and configuration.
        /// </summary>
        /// <param name="webApplicationBuilder">The web application builder.</param>
        /// <param name="jwtConfiguration">The jwt configuration.</param>
        /// <returns>The given <paramref name="webApplicationBuilder" />.</returns>
        public static WebApplicationBuilder AddJwtTokenService(
            this WebApplicationBuilder webApplicationBuilder,
            IJwtConfiguration jwtConfiguration
        )
        {
            if (webApplicationBuilder.Services.Any(service => service.ServiceType == typeof(IJwtConfiguration)))
            {
                return webApplicationBuilder;
            }

            webApplicationBuilder.Services.TryAddScoped<IJwtConfiguration>(_ => jwtConfiguration);
            webApplicationBuilder.Services.TryAddScoped<IJwtTokenService, JwtTokenService>();

            webApplicationBuilder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey =
                                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Key)),
                            ValidateIssuerSigningKey = true,
                            ValidAudience = jwtConfiguration.Audience,
                            ValidateAudience = true,
                            ValidateIssuer = true,
                            ValidIssuer = jwtConfiguration.Issuer
                        };
                    });

            return webApplicationBuilder;
        }
    }
}
