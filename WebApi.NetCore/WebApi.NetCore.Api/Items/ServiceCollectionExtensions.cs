namespace WebApi.NetCore.Api.Items
{
    using Generic.Base.Api.Database;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;
    using MongoDB.Driver;
    using WebApi.NetCore.Api.Contracts.Configuration;
    using WebApi.NetCore.Api.Database;

    /// <summary>
    ///     Extensions for <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddItemDependencies(this IServiceCollection services)
        {
            services.AddScoped<IProviderEntryTransformer<Item, DatabaseItem>, ItemTransformer>();
            services
                .AddScoped<IProvider<Item, IClientSessionHandle>,
                    MongoDbProvider<Item, DatabaseItem, IItemDatabaseConfiguration>>();
            services.AddSingleton<IItemDatabaseConfiguration>(
                _ => new ItemDatabaseConfiguration
                {
                    CollectionName = "items",
                    DatabaseName = "warehouse"
                });

            services
                .AddScoped<IAtomicService<CreateItem, Item, UpdateItem, IClientSessionHandle>,
                    AtomicService<CreateItem, Item, UpdateItem, IClientSessionHandle>>();
            services.AddScoped<IAtomicTransformer<CreateItem, Item, UpdateItem>, ItemTransformer>();

            services
                .AddScoped<IDomainService<CreateItem, Item, UpdateItem>,
                    DomainService<CreateItem, Item, UpdateItem, IClientSessionHandle>>();

            services.AddScoped<ItemTransformer, ItemTransformer>();

            return services;
        }
    }
}
