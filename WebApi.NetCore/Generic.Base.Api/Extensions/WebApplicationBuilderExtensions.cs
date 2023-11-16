namespace Generic.Base.Api.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    ///     Extensions for <see cref="WebApplicationBuilder" />.
    /// </summary>
    public static class WebApplicationBuilderExtensions
    {
        /// <summary>
        ///     Reads a section from the configuration.
        /// </summary>
        /// <typeparam name="T">The type of the configuration section.</typeparam>
        /// <param name="webApplicationBuilder">The web application builder.</param>
        /// <param name="configurationSectionName">Name of the configuration section.</param>
        /// <returns>The requested configuration.</returns>
        /// <exception cref="ArgumentException">
        ///     The configuration file appSettings.json does not include a section called
        ///     {configurationSectionName} - configurationSectionName
        /// </exception>
        public static T ReadFromConfiguration<T>(
            this WebApplicationBuilder webApplicationBuilder,
            string configurationSectionName
        )
        {
            var configuration = webApplicationBuilder.Configuration.GetSection(configurationSectionName).Get<T>();
            if (configuration is null)
            {
                throw new ArgumentException(
                    $"The configuration file appSettings.json does not include a section called {configurationSectionName}",
                    nameof(configurationSectionName));
            }

            return configuration;
        }
    }
}
