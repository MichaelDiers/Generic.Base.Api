namespace Generic.Base.Api.Jwt
{
    /// <inheritdoc cref="IToken" />
    internal class Token : IToken
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Token" /> class.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="refreshToken">The refresh token.</param>
        public Token(string accessToken, string refreshToken)
        {
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
        }

        /// <summary>
        ///     Gets the access token.
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        ///     Gets the refresh token.
        /// </summary>
        public string RefreshToken { get; }
    }
}
