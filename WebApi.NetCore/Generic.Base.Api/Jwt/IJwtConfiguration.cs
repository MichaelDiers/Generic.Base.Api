namespace Generic.Base.Api.Jwt
{
    /// <summary>
    ///     Describes the jwt configuration.
    /// </summary>
    public interface IJwtConfiguration
    {
        /// <summary>
        ///     Gets a value that specifies after how many minutes the access token expires.
        /// </summary>
        int AccessTokenExpires { get; }

        /// <summary>
        ///     Gets the audience.
        /// </summary>
        string Audience { get; }

        /// <summary>
        ///     Gets the issuer.
        /// </summary>
        string Issuer { get; }

        /// <summary>
        ///     Gets the symmetric key.
        /// </summary>
        string Key { get; }

        /// <summary>
        ///     Gets a value that specifies after how many minutes the refresh token expires.
        /// </summary>
        int RefreshTokenExpires { get; }
    }
}
