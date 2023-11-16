namespace Generic.Base.Api.Extensions
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    ///     Extensions for <see cref="IConfiguration" />.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        ///     Reads a section from the configuration.
        /// </summary>
        /// <typeparam name="T">The type of the configuration section.</typeparam>
        /// <param name="configuration">The configuration of the application.</param>
        /// <param name="configurationSectionName">Name of the configuration section.</param>
        /// <returns>The requested configuration.</returns>
        /// <exception cref="ArgumentException">
        ///     The configuration file appSettings.json does not include a section called
        ///     {configurationSectionName} - configurationSectionName
        /// </exception>
        public static T ReadFromConfiguration<T>(this IConfiguration configuration, string configurationSectionName)
        {
            var config = configuration.GetSection(configurationSectionName).Get<T>();
            if (config is null)
            {
                throw new ArgumentException(
                    $"The configuration file appSettings.json does not include a section called {configurationSectionName}",
                    nameof(configurationSectionName));
            }

            return config;
        }
    }
}
