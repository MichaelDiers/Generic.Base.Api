namespace Generic.Base.Api.Middleware.ApiKey
{
    /// <summary>
    ///     The configuration of the api key validation.
    /// </summary>
    public interface IApiKeyConfiguration
    {
        /// <summary>
        ///     Gets the api key.
        /// </summary>
        string ApiKey { get; }
    }
}
