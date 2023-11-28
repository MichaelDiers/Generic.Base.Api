namespace Generic.Base.Api.Extensions
{
    using Generic.Base.Api.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    /// <summary>
    ///     Extensions for <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices<TCreate, TEntry, TUpdate, TClientSessionHandle>(
            this IServiceCollection services
        )
        {
            services
                .TryAddScoped<IDomainService<TCreate, TEntry, TUpdate>,
                    DomainService<TCreate, TEntry, TUpdate, TClientSessionHandle>>();
            services
                .TryAddScoped<IAtomicService<TCreate, TEntry, TUpdate, TClientSessionHandle>,
                    AtomicService<TCreate, TEntry, TUpdate, TClientSessionHandle>>();

            return services;
        }

        /// <summary>
        ///     Adds the user bound services.
        /// </summary>
        /// <typeparam name="TCreate">The data type for creating an entry.</typeparam>
        /// <typeparam name="TEntry">The type of the entry.</typeparam>
        /// <typeparam name="TUpdate">The data type for updating an entry.</typeparam>
        /// <typeparam name="TClientSessionHandle">The type of the client session handle.</typeparam>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection AddUserBoundServices<TCreate, TEntry, TUpdate, TClientSessionHandle>(
            this IServiceCollection services
        )
        {
            services
                .TryAddScoped<IUserBoundDomainService<TCreate, TEntry, TUpdate>,
                    UserBoundDomainService<TCreate, TEntry, TUpdate, TClientSessionHandle>>();
            services
                .TryAddScoped<IUserBoundAtomicService<TCreate, TEntry, TUpdate, TClientSessionHandle>,
                    UserBoundAtomicService<TCreate, TEntry, TUpdate, TClientSessionHandle>>();

            return services;
        }
    }
}
