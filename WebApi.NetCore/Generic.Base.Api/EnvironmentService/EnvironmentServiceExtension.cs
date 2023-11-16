namespace Generic.Base.Api.EnvironmentService
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    ///     Extensions for the <see cref="EnvironmentService" />.
    /// </summary>
    public static class EnvironmentServiceExtension
    {
        /// <summary>
        ///     Adds the environment service.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>An instance of <see cref="IEnvironmentService" />.</returns>
        public static IEnvironmentService AddEnvironmentService(this IServiceCollection services)
        {
            var environmentService = new EnvironmentService();
            if (services.All(service => service.ServiceType != typeof(IEnvironmentService)))
            {
                services.AddSingleton<IEnvironmentService>(environmentService);
            }

            return environmentService;
        }
    }
}
