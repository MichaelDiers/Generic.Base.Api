namespace Generic.Base.Api.MongoDb
{
    using Generic.Base.Api.Database;
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using MongoDB.Driver;

    /// <summary>
    ///     Extensions for <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds the user bound services.
        /// </summary>
        /// <typeparam name="TCreate">The data type for creating an entry.</typeparam>
        /// <typeparam name="TEntry">The type of the entry.</typeparam>
        /// <typeparam name="TUpdate">The data type for updating an entry.</typeparam>
        /// <typeparam name="TClientSessionHandle">The type of the client session handle.</typeparam>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection
            AddUserBoundServices<TCreate, TEntry, TUpdate, TResult, TClientSessionHandle, TTransformer,
                TDatabaseConfiguration>(this IServiceCollection services)
            where TResult : ILinkResult
            where TTransformer : class, IControllerTransformer<TEntry, TResult>,
            IUserBoundAtomicTransformer<TCreate, TEntry, TUpdate>
            where TDatabaseConfiguration : IDatabaseConfiguration
            where TEntry : IUserBoundEntry
        {
            services
                .TryAddScoped<IUserBoundDomainService<TCreate, TEntry, TUpdate>,
                    UserBoundDomainService<TCreate, TEntry, TUpdate, TClientSessionHandle>>();
            services
                .TryAddScoped<IUserBoundAtomicService<TCreate, TEntry, TUpdate, TClientSessionHandle>,
                    UserBoundAtomicService<TCreate, TEntry, TUpdate, TClientSessionHandle>>();

            services.TryAddScoped<IControllerTransformer<TEntry, TResult>, TTransformer>();
            services.TryAddScoped<ITransactionHandler<IClientSessionHandle>, TransactionHandler>();
            services
                .TryAddScoped<IUserBoundProvider<TEntry, IClientSessionHandle>,
                    MongoDbUserBoundProvider<TEntry, TEntry, TDatabaseConfiguration>>();
            services.TryAddScoped<IUserBoundAtomicTransformer<TCreate, TEntry, TUpdate>, TTransformer>();

            return services;
        }
    }
}
