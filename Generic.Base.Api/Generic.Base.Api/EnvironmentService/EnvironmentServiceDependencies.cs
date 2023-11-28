namespace Generic.Base.Api.EnvironmentService
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    /// <summary>
    ///     Add the dependencies of the <see cref="EnvironmentService" />.
    /// </summary>
    public static class EnvironmentServiceDependencies
    {
        /// <summary>
        ///     The environment service.
        /// </summary>
        private static IEnvironmentService? environmentService;

        /// <summary>
        ///     Adds the environment service.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        public static IServiceCollection AddEnvironmentService(this IServiceCollection services)
        {
            services.TryAddScoped<IEnvironmentService, EnvironmentService>();
            return services;
        }

        /// <summary>
        ///     Gets the environment service.
        /// </summary>
        /// <returns>An <see cref="IEnvironmentService" />.</returns>
        public static IEnvironmentService GetEnvironmentService()
        {
            EnvironmentServiceDependencies.environmentService ??= new EnvironmentService();
            return EnvironmentServiceDependencies.environmentService;
        }
    }
}
