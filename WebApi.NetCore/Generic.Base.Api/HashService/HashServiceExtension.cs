namespace Generic.Base.Api.HashService
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    ///     Add the hash service.
    /// </summary>
    public static class HashServiceExtension
    {
        /// <summary>
        ///     Adds the hash service.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        public static IServiceCollection AddHash(this IServiceCollection services)
        {
            return services.AddScoped<IHashService, HashService>();
        }
    }
}
