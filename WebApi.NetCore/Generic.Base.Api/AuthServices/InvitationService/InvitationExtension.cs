namespace Generic.Base.Api.AuthServices.InvitationService
{
    using Generic.Base.Api.Database;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    ///     Add the dependencies for using the invitation service.
    /// </summary>
    public static class InvitationExtension
    {
        /// <summary>
        ///     Adds the invitation service.
        /// </summary>
        /// <typeparam name="TClientSessionHandle">The type of the client session handle.</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="transactionHandler">The transaction handler.</param>
        /// <param name="provider">The database provider.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        public static IServiceCollection AddInvitationService<TClientSessionHandle>(
            this IServiceCollection services,
            ITransactionHandler<TClientSessionHandle> transactionHandler,
            IProvider<Invitation, TClientSessionHandle> provider
        )
        {
            services
                .AddScoped<IDomainService<Invitation, Invitation, Invitation>,
                    DomainService<Invitation, Invitation, Invitation, TClientSessionHandle>>();
            services.AddScoped<IControllerTransformer<Invitation, ResultInvitation>, InvitationTransformer>();

            services.AddScoped<ITransactionHandler<TClientSessionHandle>>(_ => transactionHandler);
            services
                .AddScoped<IAtomicService<Invitation, Invitation, Invitation, TClientSessionHandle>,
                    AtomicService<Invitation, Invitation, Invitation, TClientSessionHandle>>();

            services.AddScoped<IProvider<Invitation, TClientSessionHandle>>(_ => provider);
            services.AddScoped<IAtomicTransformer<Invitation, Invitation, Invitation>, InvitationTransformer>();

            return services;
        }
    }
}
