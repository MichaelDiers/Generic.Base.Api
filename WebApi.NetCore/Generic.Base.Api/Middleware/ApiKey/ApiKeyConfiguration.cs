namespace Generic.Base.Api.Middleware.ApiKey
{
    using System.Text.Json.Serialization;

    /// <summary>
    ///     The configuration of the api key validation.
    /// </summary>
    public class ApiKeyConfiguration : IApiKeyConfiguration
    {
        /// <summary>
        ///     The configuration section name in appSettings.json file.
        /// </summary>
        public static string ConfigurationSection = "ApiKey";

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApiKeyConfiguration" /> class.
        /// </summary>
        /// <param name="keyName">The name of the environment variable that contains the api key.</param>
        public ApiKeyConfiguration(string keyName)
        {
            this.KeyName = keyName;
            this.ApiKey = string.Empty;
        }

        /// <summary>
        ///     Gets or sets the name of the environment variable that contains the api key.
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        ///     Gets or sets the api key.
        /// </summary>
        [JsonIgnore]
        public string ApiKey { get; set; }
    }
}
