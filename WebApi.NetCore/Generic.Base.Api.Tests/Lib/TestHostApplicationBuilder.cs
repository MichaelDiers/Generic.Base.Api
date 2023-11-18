namespace Generic.Base.Api.Tests.Lib
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    ///     Initialize test dependencies.
    /// </summary>
    internal static class TestHostApplicationBuilder
    {
        /// <summary>
        ///     Gets the host.
        /// </summary>
        /// <param name="addDependencies">The add dependencies.</param>
        /// <returns>The requested service.</returns>
        public static IHost GetHost(params Func<IServiceCollection, IServiceCollection>[] addDependencies)
        {
            var builder = new HostApplicationBuilder();

            foreach (var addDependency in addDependencies)
            {
                addDependency(builder.Services);
            }

            return builder.Build();
        }

        /// <summary>
        ///     Gets the host.
        /// </summary>
        /// <param name="addDependencies">The add dependencies.</param>
        /// <returns>The requested service.</returns>
        public static IHost GetHost(params Func<WebApplicationBuilder, WebApplicationBuilder>[] addDependencies)
        {
            var builder = WebApplication.CreateBuilder();

            foreach (var addDependency in addDependencies)
            {
                addDependency(builder);
            }

            return builder.Build();
        }

        /// <summary>
        ///     Gets the service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="addDependencies">The add dependencies.</param>
        /// <returns>The requested service.</returns>
        public static TService GetService<TService>(
            params Func<IServiceCollection, IServiceCollection>[] addDependencies
        )
        {
            var app = TestHostApplicationBuilder.GetHost(addDependencies);

            var service = app.Services.GetService<TService>();
            Assert.NotNull(service);

            return service;
        }

        /// <summary>
        ///     Gets the service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="addDependencies">The add dependencies.</param>
        /// <returns>The requested service.</returns>
        public static TService GetService<TService>(
            params Func<WebApplicationBuilder, WebApplicationBuilder>[] addDependencies
        )
        {
            var app = TestHostApplicationBuilder.GetHost(addDependencies);

            var service = app.Services.GetService<TService>();
            Assert.NotNull(service);

            return service;
        }

        /// <summary>
        ///     Gets the services.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="addDependencies">The add dependencies.</param>
        /// <returns>The requested service.</returns>
        public static IEnumerable<TService> GetServices<TService>(
            params Func<IServiceCollection, IServiceCollection>[] addDependencies
        )
        {
            var app = TestHostApplicationBuilder.GetHost(addDependencies);

            var services = app.Services.GetServices<TService>().ToArray();
            Assert.NotEmpty(services);

            return services;
        }
    }
}
