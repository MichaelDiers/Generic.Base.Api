namespace Generic.Base.Api.AuthServices.AuthService
{
    using Generic.Base.Api.Database;
    using Generic.Base.Api.HashService;
    using Generic.Base.Api.Jwt;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    ///     Add the auth service dependencies.
    /// </summary>
    public static class AuthServiceDependencies
    {
        /// <summary>
        ///     Adds the authentication service.
        /// </summary>
        /// <typeparam name="TClientSessionHandle">The type of the client session handle.</typeparam>
        /// <typeparam name="TTransactionHandler">The type of the transaction handler.</typeparam>
        /// <param name="builder">The web application builder.</param>
        /// <returns>The given <paramref name="builder" />.</returns>
        public static WebApplicationBuilder AddAuthService<TClientSessionHandle, TTransactionHandler>(
            this WebApplicationBuilder builder
        ) where TTransactionHandler : class, ITransactionHandler<TClientSessionHandle>
        {
            builder.Services.AddScoped<IDomainAuthService, DomainAuthService<TClientSessionHandle>>();
            builder.Services.AddScoped<ITransactionHandler<TClientSessionHandle>, TTransactionHandler>();
            builder.Services.AddHashService();
            builder.AddJwtTokenService();

            return builder;
        }
    }
}
