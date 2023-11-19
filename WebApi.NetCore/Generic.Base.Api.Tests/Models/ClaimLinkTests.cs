namespace Generic.Base.Api.Tests.Models
{
    using System.Security.Claims;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Models;

    /// <summary>
    ///     Tests for <see cref="ClaimLink" />.
    /// </summary>
    public class ClaimLinkTests
    {
        [Fact]
        public void CanBeAccessedFailsDueToMissingRole()
        {
            const Urn urn = Urn.Delete;
            const string url = "my url";
            var claims = new[]
            {
                new Claim(
                    ClaimTypes.Role,
                    nameof(Role.Admin)),
                new Claim(
                    ClaimTypes.Role,
                    nameof(Role.Accessor))
            };

            var claimLink = new ClaimLink(
                urn,
                url,
                claims);

            Assert.Equal(
                $"urn:{urn}",
                claimLink.Urn);
            Assert.Equal(
                url,
                claimLink.Url);
            Assert.False(claimLink.CanBeAccessed(claims.Take(1)));
        }

        [Fact]
        public void CanBeAccessedSucceeds()
        {
            const Urn urn = Urn.Delete;
            const string url = "my url";
            var claims = new[]
            {
                new Claim(
                    ClaimTypes.Role,
                    nameof(Role.Admin)),
                new Claim(
                    ClaimTypes.Role,
                    nameof(Role.Accessor))
            };

            var claimLink = new ClaimLink(
                urn,
                url,
                claims);

            Assert.Equal(
                $"urn:{urn}",
                claimLink.Urn);
            Assert.Equal(
                url,
                claimLink.Url);
            Assert.True(
                claimLink.CanBeAccessed(
                    claims.Append(
                            new Claim(
                                ClaimTypes.NameIdentifier,
                                "id"))
                        .ToArray()));
        }

        [Fact]
        public void CtorWithEmptyClaims()
        {
            const Urn urn = Urn.Delete;
            const string url = "my url";
            var claims = Enumerable.Empty<Claim>();

            var claimLink = new ClaimLink(
                urn,
                url,
                claims);

            Assert.Equal(
                $"urn:{urn}",
                claimLink.Urn);
            Assert.Equal(
                url,
                claimLink.Url);
            Assert.True(claimLink.CanBeAccessed(Enumerable.Empty<Claim>()));
        }

        [Fact]
        public void CtorWithoutClaims()
        {
            const Urn urn = Urn.Delete;
            const string url = "my url";

            var claimLink = new ClaimLink(
                urn,
                url);

            Assert.Equal(
                $"urn:{urn}",
                claimLink.Urn);
            Assert.Equal(
                url,
                claimLink.Url);
            Assert.True(claimLink.CanBeAccessed(Enumerable.Empty<Claim>()));
        }

        [Fact]
        public void IsIClaimLink()
        {
            Assert.IsAssignableFrom<IClaimLink>(
                new ClaimLink(
                    Urn.Delete,
                    "url"));
        }

        [Fact]
        public void IsILink()
        {
            Assert.IsAssignableFrom<ILink>(
                new ClaimLink(
                    Urn.Delete,
                    "url"));
        }
    }
}
