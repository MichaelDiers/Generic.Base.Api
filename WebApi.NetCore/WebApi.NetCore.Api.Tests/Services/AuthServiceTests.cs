namespace WebApi.NetCore.Api.Tests.Services
{
    [Trait(
        "Type",
        "Unit")]
    public class AuthServiceTests
    {
        [Fact]
        public void SignIn()
        {
            /*
            const string token = nameof(token);
            const Role role = Role.Admin;

            var jwtTokenService = new Mock<IJwtTokenService>();
            jwtTokenService.Setup(mock => mock.CreateToken(It.IsAny<IEnumerable<Claim>>())).Returns(token);

            var authService =
                TestHostApplicationBuilder.GetService<IAuthService, IJwtTokenService>(jwtTokenService.Object);

            var result = authService.SignIn(role);

            Assert.Equal(
                token,
                result);

            jwtTokenService.Verify(
                mock => mock.CreateToken(It.Is<IEnumerable<Claim>>(value => value.Single().Value == role.ToString())),
                Times.Once);
            jwtTokenService.VerifyNoOtherCalls();
            jwtTokenService.VerifyNoOtherCalls();
            */
        }
    }
}
