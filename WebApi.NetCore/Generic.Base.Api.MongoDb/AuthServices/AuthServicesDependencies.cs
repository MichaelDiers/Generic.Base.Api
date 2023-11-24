namespace Generic.Base.Api.MongoDb.AuthServices
{
    using Generic.Base.Api.AuthServices.InvitationService;
    using Generic.Base.Api.AuthServices.TokenService;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.EnvironmentService;
    using Generic.Base.Api.MongoDb;
    using Generic.Base.Api.Transformer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using MongoDB.Driver;

    /// <summary>
    ///     Add all services needed for the auth service.
    /// </summary>
    public static class AuthServicesDependencies
    {
        /// <summary>
        ///     The mongo database connection string key.
        /// </summary>
        public const string MongoDbConnectionStringKey = "GENERIC_AUTH_SERVICES_MONGO";

        /// <summary>
        ///     Adds the authentication services.
        /// </summary>
        /// <param name="builder">The web application builder.</param>
        /// <returns>The given <paramref name="builder" />.</returns>
        public static WebApplicationBuilder AddAuthServices(this WebApplicationBuilder builder)
        {
            Api.AuthServices.AuthServicesDependencies
                .AddAuthServices<IClientSessionHandle, TransactionHandler,
                    MongoDbProvider<Invitation, Invitation, IInvitationDatabaseConfiguration>,
                    MongoDbProvider<TokenEntry, TokenEntry, ITokenEntryDatabaseConfiguration>,
                    MongoDbProvider<User, User, IUserDatabaseConfiguration>>(builder);

            builder.Services
                .TryAddSingleton<IProviderEntryTransformer<Invitation, Invitation>, EntryTransformer<Invitation>>();
            builder.Services
                .TryAddSingleton<IProviderEntryTransformer<TokenEntry, TokenEntry>, EntryTransformer<TokenEntry>>();
            builder.Services.TryAddSingleton<IProviderEntryTransformer<User, User>, EntryTransformer<User>>();

            builder.Services.TryAddSingleton<IInvitationDatabaseConfiguration>(
                _ => AuthServicesDependencies.ReadDatabaseConfiguration<MongoDbDatabaseConfiguration>(
                    builder,
                    MongoDbDatabaseConfiguration.InvitationDatabaseConfiguration));
            builder.Services.TryAddSingleton<ITokenEntryDatabaseConfiguration>(
                _ => AuthServicesDependencies.ReadDatabaseConfiguration<MongoDbDatabaseConfiguration>(
                    builder,
                    MongoDbDatabaseConfiguration.TokenEntryDatabaseConfiguration));
            builder.Services.TryAddSingleton<IUserDatabaseConfiguration>(
                _ => AuthServicesDependencies.ReadDatabaseConfiguration<MongoDbDatabaseConfiguration>(
                    builder,
                    MongoDbDatabaseConfiguration.UserDatabaseConfiguration));

            builder.Services.AddSingleton<IMongoClient>(
                _ =>
                {
                    var connectionString =
                        EnvironmentService.GetValue(AuthServicesDependencies.MongoDbConnectionStringKey);
                    return new MongoClient(connectionString);
                });

            return builder;
        }

        /// <summary>
        ///     Reads the database configuration.
        /// </summary>
        /// <typeparam name="TDatabaseConfiguration">The type of the database configuration.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns>The requested database configuration.</returns>
        /// <exception cref="System.ArgumentException">Cannot read configuration for key {sectionName}. - sectionName</exception>
        private static TDatabaseConfiguration ReadDatabaseConfiguration<TDatabaseConfiguration>(
            WebApplicationBuilder builder,
            string sectionName
        )
        {
            var configuration = builder.Configuration.GetSection(sectionName).Get<TDatabaseConfiguration>();
            if (configuration is null)
            {
                throw new ArgumentException(
                    $"Cannot read configuration for key {sectionName}.",
                    nameof(sectionName));
            }

            return configuration;
        }
    }
}
