namespace WebApi.NetCore.Api.Services
{
    using WebApi.NetCore.Api.Contracts.Services;

    /// <inheritdoc cref="IEnvironmentService" />
    internal class EnvironmentService : IEnvironmentService
    {
        /// <summary>
        ///     Gets the specified environment variable with the specified key.
        /// </summary>
        /// <param name="key">The key or name of the environment variable.</param>
        /// <returns>The value of the environment variable.</returns>
        public string Get(string key)
        {
            var value = Environment.GetEnvironmentVariable(key);

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return value;
        }
    }
}
