namespace Generic.Base.Api.Tests.Extensions
{
    using System.Security.Claims;
    using Generic.Base.Api.Extensions;

    /// <summary>
    ///     Tests for <see cref="ClaimExtensions" />.
    /// </summary>
    [Trait(
        "TestType",
        "UnitTest")]
    public class ClaimExtensionsTests
    {
        [Theory]
        [InlineData("userId")]
        public void GetUserIdExtractUserId(string userId)
        {
            var claims = new[]
            {
                new Claim(
                    ClaimTypes.Role,
                    "foo"),
                new Claim(
                    Constants.UserIdClaimType,
                    userId),
                new Claim(
                    ClaimTypes.Actor,
                    "actor")
            };

            var actual = claims.GetUserId();

            Assert.Equal(
                userId,
                actual);
        }

        [Fact]
        public void GetUserIdThrowsArgumentExceptionIfNotUserIdClaimExists()
        {
            var claims = Enumerable.Empty<Claim>();

            Assert.Throws<ArgumentException>(() => claims.GetUserId());
        }

        [Fact]
        public void IsNotRefreshTokenId()
        {
            Assert.False(
                new Claim(
                    Constants.UserIdClaimType,
                    "user").IsRefreshTokenId());
        }

        [Fact]
        public void IsNotUserId()
        {
            Assert.False(
                new Claim(
                    Constants.RefreshTokenIdClaimType,
                    "token").IsUserId());
        }

        [Fact]
        public void IsRefreshTokenId()
        {
            Assert.True(
                new Claim(
                    Constants.RefreshTokenIdClaimType,
                    "token").IsRefreshTokenId());
        }

        [Fact]
        public void IsUserId()
        {
            Assert.True(
                new Claim(
                    Constants.UserIdClaimType,
                    "user").IsUserId());
        }

        [Theory]
        [InlineData("tokenId")]
        public void TryGetRefreshTokenId(string tokenId)
        {
            var claims = new[]
            {
                new Claim(
                    ClaimTypes.Role,
                    "foo"),
                new Claim(
                    Constants.RefreshTokenIdClaimType,
                    tokenId),
                new Claim(
                    ClaimTypes.Actor,
                    "actor")
            };

            Assert.True(claims.TryGetRefreshTokenId(out var actual));
            Assert.Equal(
                tokenId,
                actual);
        }

        [Fact]
        public void TryGetRefreshTokenIdFails()
        {
            var claims = new[]
            {
                new Claim(
                    ClaimTypes.Role,
                    "foo"),
                new Claim(
                    ClaimTypes.Actor,
                    "actor")
            };

            Assert.False(claims.TryGetRefreshTokenId(out var actual));
        }

        [Fact]
        public void TryGetUserIdFails()
        {
            var claims = new[]
            {
                new Claim(
                    ClaimTypes.Role,
                    "foo"),
                new Claim(
                    ClaimTypes.Actor,
                    "actor")
            };

            Assert.False(claims.TryGetUserId(out var actual));
        }

        [Theory]
        [InlineData("userId")]
        public void TryGetUserIdId(string userId)
        {
            var claims = new[]
            {
                new Claim(
                    ClaimTypes.Role,
                    "foo"),
                new Claim(
                    Constants.UserIdClaimType,
                    userId),
                new Claim(
                    ClaimTypes.Actor,
                    "actor")
            };

            Assert.True(claims.TryGetUserId(out var actual));
            Assert.Equal(
                userId,
                actual);
        }
    }
}
