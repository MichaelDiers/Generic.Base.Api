namespace Generic.Base.Api.Jwt
{
    using System.Text.Json.Serialization;

    /// <inheritdoc cref="IJwtConfiguration" />
    public class JwtConfiguration : IJwtConfiguration
    {
        /// <summary>
        ///     The configuration section name in appSettings.json file.
        /// </summary>
        public static string ConfigurationSection = "Jwt";

        /// <summary>
        ///     Initializes a new instance of the <see cref="JwtConfiguration" /> class.
        /// </summary>
        /// <param name="audience">The audience.</param>
        /// <param name="issuer">The issuer.</param>
        /// <param name="keyName">The name of the environment variable that contains the jwt key.</param>
        public JwtConfiguration(string audience, string issuer, string keyName)

        {
            this.Audience = audience;
            this.Issuer = issuer;
            this.KeyName = keyName;
            this.Key = string.Empty;
        }

        /// <summary>
        ///     Gets or sets the name of the environment variable that contains the jwt key.
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        ///     Gets or sets a value that specifies after how many minutes the access token expires.
        /// </summary>
        public int AccessTokenExpires { get; set; }

        /// <summary>
        ///     Gets or sets the audience.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        ///     Gets or sets  the issuer.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        ///     Gets or sets the symmetric key.
        /// </summary>
        [JsonIgnore]
        public string Key { get; set; }

        /// <summary>
        ///     Gets or sets a value that specifies after how many minutes the refresh token expires.
        /// </summary>
        public int RefreshTokenExpires { get; set; }
    }
}
