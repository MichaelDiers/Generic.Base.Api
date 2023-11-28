namespace Generic.Base.Api.MongoDb
{
    using Generic.Base.Api.Database;
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using MongoDB.Driver;

    /// <summary>
    ///     Extensions for <see cref="WebApplicationBuilder" />.
    /// </summary>
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder
            AddUserBoundServices<TCreate, TEntry, TUpdate, TResult, TDatabaseEntry, TTransformer,
                TDatabaseConfiguration>(this WebApplicationBuilder builder)
            where TResult : ILinkResult
            where TTransformer : class, IControllerTransformer<TEntry, TResult>,
            IUserBoundAtomicTransformer<TCreate, TEntry, TUpdate>, IProviderEntryTransformer<TEntry, TDatabaseEntry>
            where TDatabaseConfiguration : class, IDatabaseConfiguration
            where TEntry : IUserBoundEntry
            where TDatabaseEntry : IUserBoundEntry
        {
            builder.Services
                .TryAddScoped<IUserBoundDomainService<TCreate, TEntry, TUpdate>,
                    UserBoundDomainService<TCreate, TEntry, TUpdate, IClientSessionHandle>>();
            builder.Services
                .TryAddScoped<IUserBoundAtomicService<TCreate, TEntry, TUpdate, IClientSessionHandle>,
                    UserBoundAtomicService<TCreate, TEntry, TUpdate, IClientSessionHandle>>();

            builder.Services.TryAddScoped<IControllerTransformer<TEntry, TResult>, TTransformer>();
            builder.Services.TryAddScoped<ITransactionHandler<IClientSessionHandle>, TransactionHandler>();
            builder.Services
                .TryAddScoped<IUserBoundProvider<TEntry, IClientSessionHandle>,
                    MongoDbUserBoundProvider<TEntry, TDatabaseEntry, TDatabaseConfiguration>>();
            builder.Services.TryAddScoped<IUserBoundAtomicTransformer<TCreate, TEntry, TUpdate>, TTransformer>();
            builder.Services.TryAddScoped<IProviderEntryTransformer<TEntry, TDatabaseEntry>, TTransformer>();
            builder.Services.TryAddScoped(
                _ => WebApplicationBuilderExtensions.ReadDatabaseConfiguration<TDatabaseConfiguration>(builder));

            return builder;
        }

        private static TDatabaseConfiguration ReadDatabaseConfiguration<TDatabaseConfiguration>(
            WebApplicationBuilder builder
        )
        {
            var sectionName = typeof(TDatabaseConfiguration).Name;
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
