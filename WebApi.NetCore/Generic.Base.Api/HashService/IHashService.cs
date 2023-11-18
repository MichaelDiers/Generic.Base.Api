namespace Generic.Base.Api.HashService
{
    /// <summary>
    ///     Create hashes of passwords.
    /// </summary>
    public interface IHashService
    {
        /// <summary>
        ///     Hashes the specified password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>The hashed password.</returns>
        string Hash(string password);

        /// <summary>
        ///     Verifies the specified password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="hash">The hashed password.</param>
        /// <returns>True if password and hash match and false otherwise.</returns>
        bool Verify(string password, string hash);
    }
}
