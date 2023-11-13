namespace WebApi.NetCore.Api.Tests.Services
{
    using Moq;
    using WebApi.NetCore.Api.Contracts;
    using WebApi.NetCore.Api.Contracts.Services;
    using WebApi.NetCore.Api.Tests.Lib;

    [Trait(
        "Type",
        "Unit")]
    public class AuthServiceTests
    {
        [Fact]
        public void SignIn()
        {
            const string token = nameof(token);
            const Role role = Role.Admin;

            var jwtTokenService = new Mock<IJwtTokenService>();
            jwtTokenService.Setup(mock => mock.CreateToken(It.IsAny<IEnumerable<Role>>())).Returns(token);

            var authService =
                TestHostApplicationBuilder.GetService<IAuthService, IJwtTokenService>(jwtTokenService.Object);

            var result = authService.SignIn(role);

            Assert.Equal(
                token,
                result);

            jwtTokenService.Verify(
                mock => mock.CreateToken(It.Is<IEnumerable<Role>>(value => value.Single() == role)),
                Times.Once);
            jwtTokenService.VerifyNoOtherCalls();
            jwtTokenService.VerifyNoOtherCalls();
        }
    }
}
