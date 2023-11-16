namespace Generic.Base.Api.AuthService.UserService
{
    using Generic.Base.Api.Database;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    ///     Add the necessary dependencies for using the user service.
    /// </summary>
    public static class UserServiceExtension
    {
        /// <summary>
        ///     Adds the user service.
        /// </summary>
        /// <typeparam name="TClientSessionHandle">The type of the client session handle.</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="transactionHandler">The transaction handler.</param>
        /// <param name="provider">The database provider.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        public static IServiceCollection AddUserService<TClientSessionHandle>(
            this IServiceCollection services,
            ITransactionHandler<TClientSessionHandle> transactionHandler,
            IProvider<User, TClientSessionHandle> provider
        )
        {
            services
                .AddScoped<IDomainService<User, User, User>, DomainService<User, User, User, TClientSessionHandle>>();
            services.AddScoped<IControllerTransformer<User, ResultUser>, UserTransformer>();

            services.AddScoped<ITransactionHandler<TClientSessionHandle>>(_ => transactionHandler);
            services
                .AddScoped<IAtomicService<User, User, User, TClientSessionHandle>,
                    AtomicService<User, User, User, TClientSessionHandle>>();

            services.AddScoped<IProvider<User, TClientSessionHandle>>(_ => provider);
            services.AddScoped<IAtomicTransformer<User, User, User>, UserTransformer>();

            return services;
        }
    }
}
