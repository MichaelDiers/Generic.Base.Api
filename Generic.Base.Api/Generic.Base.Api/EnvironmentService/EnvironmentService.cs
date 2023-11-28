namespace Generic.Base.Api.EnvironmentService
{
    /// <inheritdoc cref="IEnvironmentService" />
    public class EnvironmentService : IEnvironmentService
    {
        /// <summary>
        ///     Gets the specified environment variable with the specified key.
        /// </summary>
        /// <param name="key">The key or name of the environment variable.</param>
        /// <returns>The value of the environment variable.</returns>
        public string Get(string key)
        {
            return EnvironmentService.GetValue(key);
        }

        /// <summary>
        ///     Gets the value of the environment variable <paramref name="key" />.
        /// </summary>
        /// <param name="key">The name of the environment variable.</param>
        /// <returns>The value of the environment variable.</returns>
        public static string GetValue(string key)
        {
            var value = Environment.GetEnvironmentVariable(key);

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    $"Cannot access environment variable {key}.",
                    nameof(key));
            }

            return value;
        }
    }
}
