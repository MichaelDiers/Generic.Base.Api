namespace Generic.Base.Api.Models
{
    using Generic.Base.Api.Configuration;

    /// <summary>
    ///     The configuration of the api key validation.
    /// </summary>
    public class ApiKeyConfiguration : IApiKeyConfiguration
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ApiKeyConfiguration" /> class.
        /// </summary>
        /// <param name="apiKey">The api key.</param>
        public ApiKeyConfiguration(string apiKey)
        {
            this.ApiKey = apiKey;
        }

        /// <summary>
        ///     Gets or sets the api key.
        /// </summary>
        public string ApiKey { get; set; }
    }
}
