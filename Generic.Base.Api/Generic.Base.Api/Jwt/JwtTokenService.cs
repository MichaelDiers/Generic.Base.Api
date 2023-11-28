namespace Generic.Base.Api.Jwt
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Generic.Base.Api.AuthServices.UserService;
    using Microsoft.IdentityModel.Tokens;

    /// <summary>
    ///     A service for creating json web tokens.
    /// </summary>
    internal class JwtTokenService : IJwtTokenService
    {
        /// <summary>
        ///     The jwt configuration.
        /// </summary>
        private readonly IJwtConfiguration configuration;

        /// <summary>
        ///     Initializes a new instance of the <see cref="JwtTokenService" /> class.
        /// </summary>
        /// <param name="configuration">The jwt configuration.</param>
        public JwtTokenService(IJwtConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        ///     Creates the json web token.
        /// </summary>
        /// <param name="id">The identifier of the user.</param>
        /// <param name="displayName">The display name of the user.</param>
        /// <param name="claims">The claims of the user.</param>
        /// <returns>An <see cref="IToken" /> that contains access and refresh token.</returns>
        public IToken CreateToken(string id, string displayName, IEnumerable<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration.Key));
            var signingCredentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha512Signature);

            var refreshTokenId = Guid.NewGuid().ToString();
            var defaultClaims = new[]
            {
                new Claim(
                    ClaimTypes.Name,
                    displayName),
                new Claim(
                    Constants.UserIdClaimType,
                    id),
                new Claim(
                    Constants.RefreshTokenIdClaimType,
                    refreshTokenId)
            };

            var accessToken = this.CreateToken(
                defaultClaims.Concat(claims)
                .Append(
                    new Claim(
                        ClaimTypes.Role,
                        nameof(Role.Accessor))),
                this.configuration.AccessTokenExpires,
                signingCredentials,
                0);

            var refreshToken = this.CreateToken(
                defaultClaims.Append(
                    new Claim(
                        ClaimTypes.Role,
                        nameof(Role.Refresher))),
                this.configuration.RefreshTokenExpires,
                signingCredentials,
                this.configuration.AccessTokenExpires);

            return new Token(
                accessToken,
                refreshToken);
        }

        /// <summary>
        ///     Decodes the specified token without validation.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>The decoded <see cref="JwtSecurityToken" />.</returns>
        public JwtSecurityToken Decode(string token)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(token);
        }

        /// <summary>
        ///     Creates the token.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="expiresIn">The expires in minutes specification.</param>
        /// <param name="signingCredentials">The signing credentials.</param>
        /// <param name="notBefore">Is not valid before <paramref name="notBefore" /> minutes.</param>
        /// <returns>A new token.</returns>
        private string CreateToken(
            IEnumerable<Claim> claims,
            int expiresIn,
            SigningCredentials signingCredentials,
            int notBefore
        )
        {
            var accessToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresIn),
                signingCredentials: signingCredentials,
                audience: this.configuration.Audience,
                issuer: this.configuration.Issuer,
                notBefore: DateTime.UtcNow.AddMinutes(notBefore));

            return new JwtSecurityTokenHandler().WriteToken(accessToken);
        }
    }
}
