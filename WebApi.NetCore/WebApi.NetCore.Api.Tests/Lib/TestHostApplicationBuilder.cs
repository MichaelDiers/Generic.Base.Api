namespace WebApi.NetCore.Api.Tests.Lib
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using WebApi.NetCore.Api.Extensions;
    using WebApi.NetCore.Api.Models.Configuration;

    /// <summary>
    ///     Initialize the test application.
    /// </summary>
    internal static class TestHostApplicationBuilder
    {
        /// <summary>
        ///     Gets the service after application initialization.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TReplaceService">The type of the service that is replaced by a mock.</typeparam>
        /// <param name="serviceMock">The service mock.</param>
        /// <returns>The requested service.</returns>
        public static TService GetService<TService, TReplaceService>(TReplaceService serviceMock)
            where TReplaceService : class
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json")
                .Build()
                .Get<AppConfiguration>();

            Assert.NotNull(configuration);

            var builder = new HostApplicationBuilder();
            builder.Services.AddDependencies();
            builder.Services.AddConfiguration(configuration);

            var serviceDescriptions = builder.Services.Where(x => x.ServiceType == typeof(TReplaceService)).ToArray();
            foreach (var serviceDescription in serviceDescriptions)
            {
                builder.Services.Remove(serviceDescription);
            }

            builder.Services.AddSingleton<TReplaceService>(_ => serviceMock);

            var app = builder.Build();

            var service = app.Services.GetService<TService>();
            Assert.NotNull(service);

            return service;
        }
    }
}
