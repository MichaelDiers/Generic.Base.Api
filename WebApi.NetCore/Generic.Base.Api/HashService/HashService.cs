namespace Generic.Base.Api.HashService
{
    using BCrypt.Net;

    /// <inheritdoc cref="IHashService" />
    internal class HashService : IHashService
    {
        /// <summary>
        ///     Hashes the specified password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>
        ///     The hashed password.
        /// </returns>
        public string Hash(string password)
        {
            return BCrypt.EnhancedHashPassword(password);
        }

        /// <summary>
        ///     Verifies the specified password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="hash">The hashed password.</param>
        /// <returns>
        ///     True if password and hash match and false otherwise.
        /// </returns>
        public bool Verify(string password, string hash)
        {
            return BCrypt.EnhancedVerify(
                password,
                hash);
        }
    }
}
