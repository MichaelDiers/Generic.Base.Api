namespace Generic.Base.Api.Jwt
{
    /// <summary>
    ///     The token pair of access and refresh token.
    /// </summary>
    public interface IToken
    {
        /// <summary>
        ///     Gets the access token.
        /// </summary>
        string AccessToken { get; }

        /// <summary>
        ///     Gets the refresh token.
        /// </summary>
        string RefreshToken { get; }
    }
}
