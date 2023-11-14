namespace WebApi.NetCore.Api.Extensions
{
    using System.Text;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using WebApi.NetCore.Api.Contracts.Configuration;
    using WebApi.NetCore.Api.Contracts.Services;
    using WebApi.NetCore.Api.Models.Configuration;
    using WebApi.NetCore.Api.Services;

    /// <summary>
    ///     Extensions for <seealso cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds the application configuration.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <param name="appConfiguration">The application configuration.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        /// <exception cref="System.ArgumentNullException">appConfiguration</exception>
        public static IServiceCollection AddConfiguration(
            this IServiceCollection services,
            IAppConfiguration? appConfiguration
        )
        {
            if (appConfiguration is null)
            {
                throw new ArgumentNullException(nameof(appConfiguration));
            }

            services.AddSingleton<IAppConfiguration>(_ => appConfiguration);
            services.AddSingleton<IJwtConfiguration>(_ => appConfiguration.Jwt);
            services.AddSingleton<IHealthCheckConfiguration>(_ => appConfiguration.HealthCheck);

            return services;
        }

        /// <summary>
        ///     Adds the dependencies.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            return services;
        }

        /// <summary>
        ///     Adds the environment information.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="envNames">The environment names configuration.</param>
        /// <returns>The created environment configuration.</returns>
        public static IEnvConfiguration AddEnvEnvironment(
            this IServiceCollection services,
            IEnvNameConfiguration? envNames
        )
        {
            if (envNames is null)
            {
                throw new ArgumentNullException(nameof(envNames));
            }

            var service = new EnvironmentService();

            var envConfiguration = new EnvConfiguration
            {
                ApiKey = service.Get(envNames.ApiKeyName),
                JwtKey = service.Get(envNames.JwtKeyName)
            };

            services.AddSingleton<IEnvConfiguration>(_ => envConfiguration);

            return envConfiguration;
        }

        /// <summary>
        ///     Adds the jwt authentication.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <param name="jwtConfiguration">The jwt configuration.</param>
        /// <param name="envConfiguration">The environment configuration.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        /// <exception cref="System.ArgumentException">key</exception>
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IJwtConfiguration? jwtConfiguration,
            IEnvConfiguration envConfiguration
        )
        {
            if (jwtConfiguration is null)
            {
                throw new ArgumentNullException(nameof(jwtConfiguration));
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey =
                                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(envConfiguration.JwtKey)),
                            ValidateIssuerSigningKey = true,
                            ValidAudience = jwtConfiguration.Audience,
                            ValidateAudience = true,
                            ValidateIssuer = true,
                            ValidIssuer = jwtConfiguration.Issuer
                        };
                    });

            return services;
        }

        /// <summary>
        ///     Adds information for swagger generator.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        public static IServiceCollection AddSwaggerGenInfo(this IServiceCollection services)
        {
            return services.AddSwaggerGen(
                option =>
                {
                    option.SwaggerDoc(
                        "v1",
                        new OpenApiInfo
                        {
                            Title = "Demo API",
                            Version = "v1"
                        });
                    option.AddSecurityDefinition(
                        "Bearer",
                        new OpenApiSecurityScheme
                        {
                            In = ParameterLocation.Header,
                            Description = "Please enter a valid token",
                            Name = "Authorization",
                            Type = SecuritySchemeType.Http,
                            BearerFormat = "JWT",
                            Scheme = "Bearer"
                        });
                    option.AddSecurityRequirement(
                        new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                Array.Empty<string>()
                            }
                        });
                });
        }
    }
}
