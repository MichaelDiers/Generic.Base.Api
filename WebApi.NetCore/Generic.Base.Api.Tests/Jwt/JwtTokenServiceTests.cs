namespace Generic.Base.Api.Tests.Jwt
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using Generic.Base.Api.Jwt;
    using Generic.Base.Api.Tests.Lib;

    public class JwtTokenServiceTests
    {
        [Fact]
        public void CreateToken()
        {
            const string id = nameof(id);
            const string displayName = nameof(displayName);

            var defaultClaims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    id),
                new Claim(
                    ClaimTypes.Name,
                    displayName)
            };

            var roleClaims = new[]
            {
                new Claim(
                    ClaimTypes.Role,
                    "Admin"),
                new Claim(
                    ClaimTypes.Role,
                    "en")
            };

            Environment.SetEnvironmentVariable(
                "GOOGLE_SECRET_MANAGER_JWT_KEY",
                "Yh2k7QSu4l8CZg5p6X3Pna9L0Miy4D3Bvt0JVr87UcOj69Kqw5R2Nmf4FWs03Hdx");

            var service =
                TestHostApplicationBuilder.GetService<IJwtTokenService>(JwtTokenServiceDependencies.AddJwtTokenService);

            var token = service.CreateToken(
                id,
                displayName,
                roleClaims);

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token.AccessToken);

            Assert.Equal(
                "audience",
                jwtSecurityToken.Audiences.Single());
            Assert.Equal(
                "issuer",
                jwtSecurityToken.Issuer);

            foreach (var claim in defaultClaims.Concat(roleClaims)
                     .Append(
                         new Claim(
                             ClaimTypes.Role,
                             "Accessor")))
            {
                Assert.Contains(
                    jwtSecurityToken.Claims,
                    c => c.Type == claim.Type && c.Value == claim.Value);
            }

            jwtSecurityToken = handler.ReadJwtToken(token.RefreshToken);

            Assert.Equal(
                "audience",
                jwtSecurityToken.Audiences.Single());
            Assert.Equal(
                "issuer",
                jwtSecurityToken.Issuer);

            foreach (var claim in defaultClaims.Append(
                         new Claim(
                             ClaimTypes.Role,
                             "Refresher")))
            {
                Assert.Contains(
                    jwtSecurityToken.Claims,
                    c => c.Type == claim.Type && c.Value == claim.Value);
            }

            Assert.Contains(
                jwtSecurityToken.Claims,
                claim => claim.Type == ClaimTypes.Sid &&
                         Guid.TryParse(
                             claim.Value,
                             out var guid) &&
                         guid != Guid.Empty);
        }
    }
}
