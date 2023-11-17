namespace Generic.Base.Api.AuthServices.TokenService
{
    using Generic.Base.Api.Database;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    ///     Add the dependencies for using the token entry service.
    /// </summary>
    public static class TokenServiceExtension
    {
        /// <summary>
        ///     Adds the token entry service.
        /// </summary>
        /// <typeparam name="TClientSessionHandle">The type of the client session handle.</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="transactionHandler">The transaction handler.</param>
        /// <param name="provider">The database provider.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        public static IServiceCollection AddTokenService<TClientSessionHandle>(
            this IServiceCollection services,
            ITransactionHandler<TClientSessionHandle> transactionHandler,
            IProvider<TokenEntry, TClientSessionHandle> provider
        )
        {
            services
                .AddScoped<IDomainService<TokenEntry, TokenEntry, TokenEntry>,
                    DomainService<TokenEntry, TokenEntry, TokenEntry, TClientSessionHandle>>();
            services.AddScoped<IControllerTransformer<TokenEntry, ResultTokenEntry>, TokenEntryTransformer>();

            services.AddScoped<ITransactionHandler<TClientSessionHandle>>(_ => transactionHandler);
            services
                .AddScoped<IAtomicService<TokenEntry, TokenEntry, TokenEntry, TClientSessionHandle>,
                    AtomicService<TokenEntry, TokenEntry, TokenEntry, TClientSessionHandle>>();

            services.AddScoped<IProvider<TokenEntry, TClientSessionHandle>>(_ => provider);
            services.AddScoped<IAtomicTransformer<TokenEntry, TokenEntry, TokenEntry>, TokenEntryTransformer>();

            return services;
        }
    }
}
