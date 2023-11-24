namespace Generic.Base.Api.AuthServices.InvitationService
{
    using Generic.Base.Api.Database;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    ///     Add the dependencies for using the invitation service.
    /// </summary>
    public static class InvitationServiceDependencies
    {
        /// <summary>
        ///     Adds the invitation service.
        /// </summary>
        /// <typeparam name="TClientSessionHandle">The type of the client session handle.</typeparam>
        /// <typeparam name="TTransactionHandler">The type of the database transaction handler.</typeparam>
        /// <typeparam name="TProvider">The type of the invitation provider.</typeparam>
        /// <param name="services">The services.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        public static IServiceCollection AddInvitationService<TClientSessionHandle, TTransactionHandler, TProvider>(
            this IServiceCollection services
        )
            where TTransactionHandler : class, ITransactionHandler<TClientSessionHandle>
            where TProvider : class, IProvider<Invitation, TClientSessionHandle>
        {
            services
                .AddScoped<IDomainService<Invitation, Invitation, Invitation>,
                    DomainService<Invitation, Invitation, Invitation, TClientSessionHandle>>();
            services.AddScoped<IControllerTransformer<Invitation, ResultInvitation>, InvitationTransformer>();

            services.AddScoped<ITransactionHandler<TClientSessionHandle>, TTransactionHandler>();
            services
                .AddScoped<IAtomicService<Invitation, Invitation, Invitation, TClientSessionHandle>,
                    AtomicService<Invitation, Invitation, Invitation, TClientSessionHandle>>();

            services.AddSingleton<IProvider<Invitation, TClientSessionHandle>, TProvider>();
            services.AddScoped<IAtomicTransformer<Invitation, Invitation, Invitation>, InvitationTransformer>();

            return services;
        }

        /// <summary>
        ///     Adds the invitation service.
        /// </summary>
        /// <typeparam name="TClientSessionHandle">The type of the client session handle.</typeparam>
        /// <typeparam name="TTransactionHandler">The type of the database transaction handler.</typeparam>
        /// <param name="services">The services.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        public static IServiceCollection AddInvitationService<TClientSessionHandle, TTransactionHandler>(
            this IServiceCollection services
        ) where TTransactionHandler : class, ITransactionHandler<TClientSessionHandle>
        {
            services
                .AddScoped<IDomainService<Invitation, Invitation, Invitation>,
                    DomainService<Invitation, Invitation, Invitation, TClientSessionHandle>>();
            services.AddScoped<IControllerTransformer<Invitation, ResultInvitation>, InvitationTransformer>();

            services.AddScoped<ITransactionHandler<TClientSessionHandle>, TTransactionHandler>();
            services
                .AddScoped<IAtomicService<Invitation, Invitation, Invitation, TClientSessionHandle>,
                    AtomicService<Invitation, Invitation, Invitation, TClientSessionHandle>>();

            services.AddScoped<IAtomicTransformer<Invitation, Invitation, Invitation>, InvitationTransformer>();

            return services;
        }
    }
}
