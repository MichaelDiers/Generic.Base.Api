namespace Generic.Base.Api.AuthServices.UserService
{
    using Generic.Base.Api.Database;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    /// <summary>
    ///     Add the necessary dependencies for using the user service.
    /// </summary>
    public static class UserServiceDependencies
    {
        /// <summary>
        ///     Adds the user service.
        /// </summary>
        /// <typeparam name="TClientSessionHandle">The type of the client session handle.</typeparam>
        /// <typeparam name="TTransactionHandler">The type of the database transaction handler.</typeparam>
        /// <typeparam name="TProvider">The type of the user provider.</typeparam>
        /// <param name="services">The services.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        public static IServiceCollection AddUserService<TClientSessionHandle, TTransactionHandler, TProvider>(
            this IServiceCollection services
        )
            where TTransactionHandler : class, ITransactionHandler<TClientSessionHandle>
            where TProvider : class, IProvider<User, TClientSessionHandle>
        {
            services
                .AddScoped<IDomainService<User, User, User>, DomainService<User, User, User, TClientSessionHandle>>();
            services.AddScoped<IControllerTransformer<User, ResultUser>, UserTransformer>();

            services.AddScoped<ITransactionHandler<TClientSessionHandle>, TTransactionHandler>();
            services
                .AddScoped<IAtomicService<User, User, User, TClientSessionHandle>,
                    AtomicService<User, User, User, TClientSessionHandle>>();

            services.TryAddSingleton<IProvider<User, TClientSessionHandle>, TProvider>();
            services.AddScoped<IAtomicTransformer<User, User, User>, UserTransformer>();

            return services;
        }

        /// <summary>
        ///     Adds the user service.
        /// </summary>
        /// <typeparam name="TClientSessionHandle">The type of the client session handle.</typeparam>
        /// <typeparam name="TTransactionHandler">The type of the database transaction handler.</typeparam>
        /// <param name="services">The services.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        public static IServiceCollection AddUserService<TClientSessionHandle, TTransactionHandler>(
            this IServiceCollection services
        ) where TTransactionHandler : class, ITransactionHandler<TClientSessionHandle>
        {
            services
                .AddScoped<IDomainService<User, User, User>, DomainService<User, User, User, TClientSessionHandle>>();
            services.AddScoped<IControllerTransformer<User, ResultUser>, UserTransformer>();

            services.AddScoped<ITransactionHandler<TClientSessionHandle>, TTransactionHandler>();
            services
                .AddScoped<IAtomicService<User, User, User, TClientSessionHandle>,
                    AtomicService<User, User, User, TClientSessionHandle>>();

            services.TryAddSingleton<IProvider<User, TClientSessionHandle>>();
            services.AddScoped<IAtomicTransformer<User, User, User>, UserTransformer>();

            return services;
        }
    }
}
