namespace WebApi.NetCore.Api.Extensions
{
    using System.Reflection;
    using Generic.Base.Api.Database;
    using Generic.Base.Api.EnvironmentService;
    using Generic.Base.Api.Models;
    using Microsoft.OpenApi.Models;
    using MongoDB.Driver;
    using WebApi.NetCore.Api.Contracts.Configuration;
    using WebApi.NetCore.Api.Contracts.Services;
    using WebApi.NetCore.Api.Database;
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

            services.AddSingleton<IDocumentationHealthCheckConfiguration>(
                _ => appConfiguration.DocumentationHealthCheck);

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

            services.AddSingleton<IMongoClient>(
                _ => new MongoClient("mongodb://localhost:27017/?replicaSet=warehouse_replSet"));
            services.AddSingleton<ITransactionHandler<IClientSessionHandle>, TransactionHandler>();

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

            var service = EnvironmentServiceDependencies.GetEnvironmentService();

            var envConfiguration = new EnvConfiguration
            {
                ApiKey = service.Get(envNames.ApiKeyName),
                JwtKey = service.Get(envNames.JwtKeyName)
            };

            services.AddSingleton<IEnvConfiguration>(_ => envConfiguration);

            return envConfiguration;
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
                    option.AddSecurityDefinition(
                        "ApiKey",
                        new OpenApiSecurityScheme
                        {
                            Description = "ApiKey must appear in header",
                            Type = SecuritySchemeType.ApiKey,
                            Name = "x-api-key",
                            In = ParameterLocation.Header,
                            Scheme = "ApiKeyScheme"
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
                    option.AddSecurityRequirement(
                        new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "ApiKey"
                                    },
                                    In = ParameterLocation.Header
                                },
                                new List<string>()
                            }
                        });

                    var xmlFilename = $"{typeof(ILink).Assembly.GetName().Name}.xml";
                    option.IncludeXmlComments(
                        Path.Combine(
                            AppContext.BaseDirectory,
                            xmlFilename),
                        true);

                    xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    option.IncludeXmlComments(
                        Path.Combine(
                            AppContext.BaseDirectory,
                            xmlFilename),
                        true);
                });
        }
    }
}
