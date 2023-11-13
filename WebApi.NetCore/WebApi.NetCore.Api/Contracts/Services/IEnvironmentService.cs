namespace WebApi.NetCore.Api.Contracts.Services
{
    /// <summary>
    ///     Service for accessing environment variables.
    /// </summary>
    public interface IEnvironmentService
    {
        /// <summary>
        ///     Gets the specified environment variable with the specified key.
        /// </summary>
        /// <param name="key">The key or name of the environment variable.</param>
        /// <returns>The value of the environment variable.</returns>
        string Get(string key);
    }
}
