﻿namespace Generic.Base.Api.HashService
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    /// <summary>
    ///     Add the hash service.
    /// </summary>
    public static class HashServiceDependencies
    {
        /// <summary>
        ///     Adds the hash service.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        public static IServiceCollection AddHashService(this IServiceCollection services)
        {
            services.TryAddScoped<IHashService, HashService>();

            return services;
        }
    }
}
