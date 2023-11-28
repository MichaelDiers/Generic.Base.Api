namespace Generic.Base.Api.Tests.Models
{
    using System.Security.Claims;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Models;

    /// <summary>
    ///     Tests for <see cref="ClaimLink" />.
    /// </summary>
    [Trait(
        "TestType",
        "UnitTest")]
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
                                Constants.UserIdClaimType,
                                "id"))
                        .ToArray()));
        }

        [Theory]
        [InlineData(
            "namespace",
            Urn.Options,
            "url",
            "type1",
            "value1",
            "type2",
            "value2")]
        public void CreateMultiClaim(
            string urnNamespace,
            Urn urn,
            string url,
            string type1,
            string value1,
            string type2,
            string value2
        )
        {
            var claim = ClaimLink.Create(
                urnNamespace,
                urn,
                url,
                new Claim(
                    type1,
                    value1),
                new Claim(
                    type2,
                    value2));

            Assert.Equal(
                url,
                claim.Url);
            Assert.Equal(
                $"urn:{urnNamespace}:{urn.ToString()}",
                claim.Urn);
        }

        [Theory]
        [InlineData(
            "namespace",
            Urn.Options,
            "url",
            "type",
            "value")]
        public void CreateSingleClaim(
            string urnNamespace,
            Urn urn,
            string url,
            string type,
            string value
        )
        {
            var claim = ClaimLink.Create(
                urnNamespace,
                urn,
                url,
                new Claim(
                    type,
                    value));

            Assert.Equal(
                url,
                claim.Url);
            Assert.Equal(
                $"urn:{urnNamespace}:{urn.ToString()}",
                claim.Urn);
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
