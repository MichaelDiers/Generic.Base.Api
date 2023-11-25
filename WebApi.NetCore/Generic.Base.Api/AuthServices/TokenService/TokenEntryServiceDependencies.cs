namespace Generic.Base.Api.AuthServices.TokenService
{
    using Generic.Base.Api.Database;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    /// <summary>
    ///     Add the dependencies for using the token entry service.
    /// </summary>
    public static class TokenEntryServiceDependencies
    {
        /// <summary>
        ///     Adds the token entry service.
        /// </summary>
        /// <typeparam name="TClientSessionHandle">The type of the client session handle.</typeparam>
        /// <typeparam name="TTransactionHandler">The type of the database transaction handler.</typeparam>
        /// <typeparam name="TProvider">The type of the token entry provider.</typeparam>
        /// <param name="services">The services.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        public static IServiceCollection AddTokenService<TClientSessionHandle, TTransactionHandler, TProvider>(
            this IServiceCollection services
        )
            where TTransactionHandler : class, ITransactionHandler<TClientSessionHandle>
            where TProvider : class, IProvider<TokenEntry, TClientSessionHandle>
        {
            services
                .TryAddScoped<IDomainService<TokenEntry, TokenEntry, TokenEntry>,
                    DomainService<TokenEntry, TokenEntry, TokenEntry, TClientSessionHandle>>();
            services.TryAddScoped<IControllerTransformer<TokenEntry, ResultTokenEntry>, TokenEntryTransformer>();

            services.TryAddScoped<ITransactionHandler<TClientSessionHandle>, TTransactionHandler>();
            services
                .TryAddScoped<IAtomicService<TokenEntry, TokenEntry, TokenEntry, TClientSessionHandle>,
                    AtomicService<TokenEntry, TokenEntry, TokenEntry, TClientSessionHandle>>();

            services.TryAddSingleton<IProvider<TokenEntry, TClientSessionHandle>, TProvider>();
            services.TryAddScoped<IAtomicTransformer<TokenEntry, TokenEntry, TokenEntry>, TokenEntryTransformer>();

            return services;
        }
    }
}
