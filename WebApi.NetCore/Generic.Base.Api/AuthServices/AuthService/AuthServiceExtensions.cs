namespace Generic.Base.Api.AuthServices.AuthService
{
    using Generic.Base.Api.Database;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    ///     Add the auth service dependencies.
    /// </summary>
    public static class AuthServiceExtensions
    {
        /// <summary>
        ///     Adds the authentication service.
        /// </summary>
        /// <typeparam name="TClientSessionHandle">The type of the client session handle.</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="transactionHandler">The transaction handler.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        public static IServiceCollection AddAuthService<TClientSessionHandle>(
            this IServiceCollection services,
            ITransactionHandler<TClientSessionHandle> transactionHandler
        )
        {
            services.AddScoped<IDomainAuthService, DomainAuthService<TClientSessionHandle>>();
            services.AddScoped<ITransactionHandler<TClientSessionHandle>>(_ => transactionHandler);

            return services;
        }
    }
}
