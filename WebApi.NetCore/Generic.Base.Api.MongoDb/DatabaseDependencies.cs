namespace Generic.Base.Api.MongoDb
{
    using Generic.Base.Api.Database;
    using Generic.Base.Api.Transformer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    /// <summary>
    ///     Add database dependencies.
    /// </summary>
    public static class DatabaseDependencies
    {
        /// <summary>
        ///     Adds the database dependencies for a given entry type.
        /// </summary>
        /// <typeparam name="TEntry">The type of the entry.</typeparam>
        /// <typeparam name="TDatabaseEntry">The type of the database entry.</typeparam>
        /// <typeparam name="TDatabaseConfiguration">The type of the database configuration.</typeparam>
        /// <typeparam name="TProviderEntryTransformer">The type of the entry transformer.</typeparam>
        /// <param name="services">The services.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        public static IServiceCollection
            AddDatabaseDependencies<TEntry, TDatabaseEntry, TDatabaseConfiguration, TProviderEntryTransformer>(
                this IServiceCollection services
            )
            where TDatabaseConfiguration : class, IDatabaseConfiguration
            where TDatabaseEntry : IIdEntry
            where TProviderEntryTransformer : class, IProviderEntryTransformer<TEntry, TDatabaseEntry>
        {
            services
                .TryAddSingleton<IDatabaseProvider<TEntry>,
                    MongoDbProvider<TEntry, TDatabaseEntry, TDatabaseConfiguration>>();
            services.AddScoped<IProviderEntryTransformer<TEntry, TDatabaseEntry>, TProviderEntryTransformer>();

            return services;
        }
    }
}
