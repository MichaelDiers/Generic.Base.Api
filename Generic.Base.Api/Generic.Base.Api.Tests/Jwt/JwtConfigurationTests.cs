namespace Generic.Base.Api.Tests.Jwt
{
    using Generic.Base.Api.Jwt;

    /// <summary>
    ///     Tests for <see cref="JwtConfiguration" />.
    /// </summary>
    [Trait(
        "TestType",
        "UnitTest")]
    public class JwtConfigurationTests
    {
        [Theory]
        [InlineData(
            "aud",
            "issuer",
            "key name",
            10,
            20)]
        public void Ctor(
            string audience,
            string issuer,
            string keyName,
            int accessTokenExpires,
            int refreshTokenExpires
        )
        {
            var configuration = new JwtConfiguration(
                audience,
                issuer,
                keyName,
                accessTokenExpires,
                refreshTokenExpires);

            Assert.Equal(
                audience,
                configuration.Audience);
            Assert.Equal(
                issuer,
                configuration.Issuer);
            Assert.Equal(
                keyName,
                configuration.KeyName);
            Assert.Equal(
                accessTokenExpires,
                configuration.AccessTokenExpires);
            Assert.Equal(
                refreshTokenExpires,
                configuration.RefreshTokenExpires);

            Assert.True(string.IsNullOrWhiteSpace(configuration.Key));

            Assert.IsAssignableFrom<IJwtConfiguration>(configuration);
        }
    }
}
