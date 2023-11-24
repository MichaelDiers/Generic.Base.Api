namespace WebApi.NetCore.Api.Tests.Integration
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using Generic.Base.Api.AuthServices.AuthService;
    using Generic.Base.Api.AuthServices.InvitationService;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Models;
    using Microsoft.AspNetCore.Mvc.Testing;

    public class AuthTests
    {
        [Fact]
        public void Foo()
        {
            var x = new WebApplicationFactory<Program>().CreateClient();
        }

        [Fact]
        public async Task Test()
        {
            Factory factory = new();
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Add(
                "x-api-key",
                "foobar");
            var response = await client.PostAsync(
                "/api/AuthController2/signInAs/myid/Admin",
                null);
            response.EnsureSuccessStatusCode();
            var tokens = await response.Content.ReadFromJsonAsync<Token>();
            Assert.NotNull(tokens);
            client.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValue.Parse($"Bearer {tokens.AccessToken}");

            var invitationOptions = await client.SendAsync(
                new HttpRequestMessage(
                    HttpMethod.Options,
                    "/api/Invitation"));
            invitationOptions.EnsureSuccessStatusCode();
            var invitationOptionsLinks = await invitationOptions.Content.ReadFromJsonAsync<LinkResult>();

            var invitation = new Invitation(
                Guid.NewGuid().ToString(),
                new[] {Role.User});

            var invitationResponse = await client.PostAsync(
                invitationOptionsLinks.Links.First(x => x.Urn == $"urn:{Urn.Create}").Url,
                JsonContent.Create(invitation));
            invitationResponse.EnsureSuccessStatusCode();

            client.DefaultRequestHeaders.Authorization = null;

            var authOptions = await client.SendAsync(
                new HttpRequestMessage(
                    HttpMethod.Options,
                    "/api/Auth"));
            var authOptionsLinks = await authOptions.Content.ReadFromJsonAsync<LinkResult>();

            var signUp = new SignUp(
                Guid.NewGuid().ToString(),
                invitation.Id,
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString());
            var signUpResult = await client.PostAsync(
                authOptionsLinks.Links.First(link => link.Urn == $"urn:{Urn.SignUp}").Url,
                JsonContent.Create(signUp));
            signUpResult.EnsureSuccessStatusCode();

            var signUpRepeatResult = await client.PostAsync(
                authOptionsLinks.Links.First(link => link.Urn == $"urn:{Urn.SignUp}").Url,
                JsonContent.Create(
                    new SignUp(
                        Guid.NewGuid().ToString(),
                        signUp.InvitationCode,
                        Guid.NewGuid().ToString(),
                        Guid.NewGuid().ToString())));
            Assert.Equal(
                HttpStatusCode.Unauthorized,
                signUpRepeatResult.StatusCode);

            var signIn = new SignIn(
                signUp.Password,
                signUp.Id);
            var signInResult = await client.PostAsync(
                authOptionsLinks.Links.First(link => link.Urn == $"urn:{Urn.SignIn}").Url,
                JsonContent.Create(signIn));
            signInResult.EnsureSuccessStatusCode();

            var signInTokens = await signInResult.Content.ReadFromJsonAsync<Token>();

            client.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValue.Parse($"Bearer {signInTokens.AccessToken}");

            var changePassword = new ChangePassword(
                signIn.Password,
                signIn.Password + "foobar");
            var changePasswordResult = await client.PostAsync(
                authOptionsLinks.Links.First(link => link.Urn == $"urn:{Urn.ChangePassword}").Url,
                JsonContent.Create(changePassword));
            changePasswordResult.EnsureSuccessStatusCode();

            var accessToken = new JwtSecurityTokenHandler().ReadJwtToken(signInTokens.AccessToken);
            var refreshToken = new JwtSecurityTokenHandler().ReadJwtToken(signInTokens.RefreshToken);

            client.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValue.Parse($"Bearer {signInTokens.RefreshToken}");
            var timespan = (int) refreshToken.ValidFrom.Subtract(DateTime.UtcNow).TotalMilliseconds;
            if (timespan > 0)
            {
                await Task.Delay(timespan + 2);
            }

            var refreshResult = await client.PostAsync(
                authOptionsLinks.Links.First(link => link.Urn == $"urn:{Urn.Refresh}").Url,
                null);
            refreshResult.EnsureSuccessStatusCode();
            var refreshedTokens = await refreshResult.Content.ReadFromJsonAsync<Token>();

            client.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValue.Parse($"Bearer {refreshedTokens.AccessToken}");

            var deleteSignIn = new SignIn(
                changePassword.NewPassword,
                signIn.Id);
            var deleteResult = await client.SendAsync(
                new HttpRequestMessage(
                    HttpMethod.Delete,
                    authOptionsLinks.Links.First(link => link.Urn == $"urn:{Urn.Delete}").Url)
                {
                    Content = JsonContent.Create(deleteSignIn)
                });
            deleteResult.EnsureSuccessStatusCode();
        }
    }
}
