namespace WebApi.NetCore.Api.Contracts.Configuration
{
    /// <summary>
    ///     The names used in <see cref="IEnvConfiguration" />.
    /// </summary>
    public interface IEnvNameConfiguration
    {
        /// <summary>
        ///     Gets the API key.
        /// </summary>
        string ApiKeyName { get; }

        /// <summary>
        ///     Gets the JWT key.
        /// </summary>
        string JwtKeyName { get; }
    }
}
