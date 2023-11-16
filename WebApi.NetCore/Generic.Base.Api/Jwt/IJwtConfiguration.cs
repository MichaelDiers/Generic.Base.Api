namespace Generic.Base.Api.Jwt
{
    /// <summary>
    ///     Describes the jwt configuration.
    /// </summary>
    public interface IJwtConfiguration
    {
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
    }
}
