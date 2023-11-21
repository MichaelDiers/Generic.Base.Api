namespace Generic.Base.Api.AuthServices
{
    using Generic.Base.Api.AuthServices.AuthService;
    using Generic.Base.Api.AuthServices.InvitationService;
    using Generic.Base.Api.AuthServices.TokenService;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Database;
    using Microsoft.AspNetCore.Builder;

    /// <summary>
    ///     Add all services needed for the auth service.
    /// </summary>
    public static class AuthServicesDependencies
    {
        /// <summary>
        ///     Adds the authentication services.
        /// </summary>
        /// <typeparam name="TClientSessionHandle">The type of the client session handle.</typeparam>
        /// <typeparam name="TTransactionHandler">The type of the transaction handler.</typeparam>
        /// <typeparam name="TInvitationProvider">The type of the invitation provider.</typeparam>
        /// <typeparam name="TTokenEntryProvider">The type of the token entry provider.</typeparam>
        /// <typeparam name="TUserProvider">The type of the user provider.</typeparam>
        /// <param name="builder">The web application builder.</param>
        /// <returns>The given <paramref name="builder" />.</returns>
        public static WebApplicationBuilder
            AddAuthServices<TClientSessionHandle, TTransactionHandler, TInvitationProvider, TTokenEntryProvider,
                TUserProvider>(this WebApplicationBuilder builder)
            where TTransactionHandler : class, ITransactionHandler<TClientSessionHandle>
            where TInvitationProvider : class, IProvider<Invitation, TClientSessionHandle>
            where TTokenEntryProvider : class, IProvider<TokenEntry, TClientSessionHandle>
            where TUserProvider : class, IProvider<User, TClientSessionHandle>
        {
            builder.AddAuthService<TClientSessionHandle, TTransactionHandler>();
            builder.Services.AddInvitationService<TClientSessionHandle, TTransactionHandler, TInvitationProvider>();
            builder.Services.AddTokenService<TClientSessionHandle, TTransactionHandler, TTokenEntryProvider>();
            builder.Services.AddUserService<TClientSessionHandle, TTransactionHandler, TUserProvider>();

            return builder;
        }
    }
}
