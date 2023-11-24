namespace Generic.Base.Api.MongoDb.Tests.IntegrationTests
{
    using System.Net;
    using Generic.Base.Api.AuthServices.AuthService;
    using Generic.Base.Api.AuthServices.InvitationService;
    using Generic.Base.Api.AuthServices.TokenService;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Jwt;
    using Generic.Base.Api.Models;
    using Generic.Base.Api.MongoDb.Tests.IntegrationTests.CustomWebApplicationFactory;

    [Trait(
        "TestType",
        "MongoDbIntegrationTest")]
    public class AuthControllerTests
    {
        public static IEnumerable<object[]> ChangePasswordTestData =>
            new[]
            {
                // default test should pass
                AuthControllerTests.CreateSignUpTestData("default test"),
                // invalid api key
                AuthControllerTests.CreateSignUpTestData(
                    "api key empty",
                    string.Empty,
                    expectedStatusCode: HttpStatusCode.Unauthorized),
                AuthControllerTests.CreateSignUpTestData(
                    "api key mismatch",
                    Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // invalid old password
                AuthControllerTests.CreateSignUpTestData(
                    "old password too short",
                    changePasswordOldPassword: new string(
                        'a',
                        7),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                AuthControllerTests.CreateSignUpTestData(
                    "old password too large",
                    changePasswordOldPassword: new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                // invalid new password
                AuthControllerTests.CreateSignUpTestData(
                    "new password too short",
                    changePasswordNewPassword: new string(
                        'a',
                        7),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                AuthControllerTests.CreateSignUpTestData(
                    "new password too large",
                    changePasswordNewPassword: new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest)
            };

        public static IEnumerable<object[]> DeleteTestData =>
            new[]
            {
                // default test should pass
                AuthControllerTests.CreateSignUpTestData("default test"),
                // invalid api key
                AuthControllerTests.CreateSignUpTestData(
                    "api key empty",
                    string.Empty,
                    expectedStatusCode: HttpStatusCode.Unauthorized),
                AuthControllerTests.CreateSignUpTestData(
                    "api key mismatch",
                    Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // invalid password
                AuthControllerTests.CreateSignUpTestData(
                    "password too short",
                    deletePassword: new string(
                        'a',
                        7),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                AuthControllerTests.CreateSignUpTestData(
                    "password too large",
                    deletePassword: new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                // invalid id
                AuthControllerTests.CreateSignUpTestData(
                    "id too short",
                    deleteId: "a",
                    expectedStatusCode: HttpStatusCode.BadRequest),
                AuthControllerTests.CreateSignUpTestData(
                    "id too large",
                    deleteId: new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest)
            };

        public static IEnumerable<object[]> RefreshTestData =>
            new[]
            {
                // default test should pass
                AuthControllerTests.CreateSignUpTestData("default test")
            };

        public static IEnumerable<object[]> SignInTestData =>
            new[]
            {
                // default test should pass
                AuthControllerTests.CreateSignUpTestData("default test"),
                // invalid api key
                AuthControllerTests.CreateSignUpTestData(
                    "api key empty",
                    string.Empty,
                    expectedStatusCode: HttpStatusCode.Unauthorized),
                AuthControllerTests.CreateSignUpTestData(
                    "api key mismatch",
                    Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // invalid password
                AuthControllerTests.CreateSignUpTestData(
                    "password too short",
                    signUpPassword: new string(
                        'a',
                        7),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                AuthControllerTests.CreateSignUpTestData(
                    "password too large",
                    signUpPassword: new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                // invalid id
                AuthControllerTests.CreateSignUpTestData(
                    "id too short",
                    signUpId: "a",
                    expectedStatusCode: HttpStatusCode.BadRequest),
                AuthControllerTests.CreateSignUpTestData(
                    "id too large",
                    signUpId: new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest)
            };

        public static IEnumerable<object[]> SignUpTestData =>
            new[]
            {
                // default test should pass
                AuthControllerTests.CreateSignUpTestData("default test"),
                // invalid api key
                AuthControllerTests.CreateSignUpTestData(
                    "api key empty",
                    string.Empty,
                    expectedStatusCode: HttpStatusCode.Unauthorized),
                AuthControllerTests.CreateSignUpTestData(
                    "api key mismatch",
                    Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // invalid display name
                AuthControllerTests.CreateSignUpTestData(
                    "display name too short",
                    signUpDisplayName: "a",
                    expectedStatusCode: HttpStatusCode.BadRequest),
                AuthControllerTests.CreateSignUpTestData(
                    "display name too large",
                    signUpDisplayName: new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                // invalid invitation id
                AuthControllerTests.CreateSignUpTestData(
                    "invitation id too short",
                    signUpInvitationCode: "a",
                    expectedStatusCode: HttpStatusCode.BadRequest),
                AuthControllerTests.CreateSignUpTestData(
                    "invitation id too large",
                    signUpInvitationCode: new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                // invalid password
                AuthControllerTests.CreateSignUpTestData(
                    "password too short",
                    signUpPassword: new string(
                        'a',
                        7),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                AuthControllerTests.CreateSignUpTestData(
                    "password too large",
                    signUpPassword: new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                // invalid id
                AuthControllerTests.CreateSignUpTestData(
                    "id too short",
                    signUpId: "a",
                    expectedStatusCode: HttpStatusCode.BadRequest),
                AuthControllerTests.CreateSignUpTestData(
                    "id too large",
                    signUpId: new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest)
            };

        [Theory]
        [MemberData(nameof(AuthControllerTests.ChangePasswordTestData))]
        public async Task ChangePassword(
            string description,
            string apiKey,
            Invitation invitation,
            SignUp signUp,
            HttpStatusCode expectedStatusCode,
            SignIn signIn,
            ChangePassword changePassword,
            params object[] other
        )
        {
            Assert.NotNull(other);

            if ((int) expectedStatusCode > 199 && (int) expectedStatusCode < 300)
            {
                // sign up
                var client = await AuthControllerTests.SignUpTest(
                    description,
                    apiKey,
                    invitation,
                    signUp,
                    expectedStatusCode);

                // change password fails without id claim
                await client.AddApiKey(apiKey)
                .AddToken(Role.Accessor)
                .PostAsync<ChangePassword, Token>(
                    changePassword,
                    () => HttpClientExtensions.GetUrl(
                        nameof(AuthController)[..^10],
                        Urn.ChangePassword),
                    HttpStatusCode.Unauthorized,
                    (_, _) => { });

                // sign in
                IToken? tokens = await client.PostAsync<SignIn, Token>(
                    signIn,
                    () => HttpClientExtensions.GetUrl(
                        nameof(AuthController)[..^10],
                        Urn.SignIn),
                    HttpStatusCode.OK,
                    (_, _) => { });
                Assert.NotNull(tokens);

                // change password without token
                await client.AddApiKey(apiKey)
                .RemoveToken()
                .PostAsync<ChangePassword, Token>(
                    changePassword,
                    () => HttpClientExtensions.GetUrl(
                        nameof(AuthController)[..^10],
                        Urn.ChangePassword),
                    HttpStatusCode.Unauthorized,
                    (_, _) => { });

                // change password using refresh token
                await client.AddApiKey(apiKey)
                    .AddToken(tokens.RefreshToken)
                    .PostAsync<ChangePassword, Token>(
                        changePassword,
                        () => HttpClientExtensions.GetUrl(
                            nameof(AuthController)[..^10],
                            Urn.ChangePassword),
                        HttpStatusCode.Unauthorized,
                        (_, _) => { });

                // change password using access token
                await client.AddApiKey(apiKey)
                    .AddToken(tokens.AccessToken)
                    .PostAsync<ChangePassword, Token>(
                        changePassword,
                        () => HttpClientExtensions.GetUrl(
                            nameof(AuthController)[..^10],
                            Urn.ChangePassword),
                        expectedStatusCode,
                        (_, _) => { });

                // sign in using old password
                await client.PostAsync<SignIn, Token>(
                    signIn,
                    () => HttpClientExtensions.GetUrl(
                        nameof(AuthController)[..^10],
                        Urn.SignIn),
                    HttpStatusCode.Unauthorized,
                    (_, _) => { });

                // sign in using new password
                await client.PostAsync<SignIn, Token>(
                    new SignIn(
                        changePassword.NewPassword,
                        signIn.Id),
                    () => HttpClientExtensions.GetUrl(
                        nameof(AuthController)[..^10],
                        Urn.SignIn),
                    HttpStatusCode.OK,
                    (_, _) => { });
                return;
            }

            await TestFactory.GetClient()
                .AddApiKey(apiKey)
                .AddToken(
                    Role.Admin,
                    Role.Accessor)
                .PostAsync<ChangePassword, Token>(
                    changePassword,
                    () => HttpClientExtensions.GetUrl(
                        nameof(AuthController)[..^10],
                        Urn.ChangePassword),
                    expectedStatusCode,
                    (_, _) => { });
        }

        [Theory]
        [MemberData(nameof(AuthControllerTests.DeleteTestData))]
        public async Task Delete(
            string description,
            string apiKey,
            Invitation invitation,
            SignUp signUp,
            HttpStatusCode expectedStatusCode,
            SignIn signIn,
            ChangePassword _,
            SignIn deleteSignIn,
            params object[] other
        )
        {
            Assert.NotNull(other);

            // sign up
            var client = await AuthControllerTests.SignUpTest(
                description,
                TestFactory.ApiKey,
                invitation,
                signUp,
                HttpStatusCode.OK);

            // sign in
            IToken? tokens = await client.PostAsync<SignIn, Token>(
                signIn,
                () => HttpClientExtensions.GetUrl(
                    nameof(AuthController)[..^10],
                    Urn.SignIn),
                HttpStatusCode.OK,
                (_, _) => { });
            Assert.NotNull(tokens);

            if ((int) expectedStatusCode > 199 && (int) expectedStatusCode < 300)
            {
                // delete fails without token
                await client.AddApiKey(apiKey)
                .RemoveToken()
                .DeleteAsync(
                    deleteSignIn,
                    await HttpClientExtensions.GetUrl(
                        nameof(AuthController)[..^10],
                        Urn.Delete),
                    HttpStatusCode.Unauthorized);

                // delete fails without id claim
                await client.AddApiKey(apiKey)
                .AddToken(Role.Accessor)
                .DeleteAsync(
                    deleteSignIn,
                    await HttpClientExtensions.GetUrl(
                        nameof(AuthController)[..^10],
                        Urn.Delete),
                    HttpStatusCode.Unauthorized);
            }

            // delete 
            await client.AddApiKey(apiKey)
            .AddToken(tokens.AccessToken)
            .DeleteAsync(
                deleteSignIn,
                await HttpClientExtensions.GetUrl(
                    nameof(AuthController)[..^10],
                    Urn.Delete),
                expectedStatusCode);

            if ((int) expectedStatusCode > 199 && (int) expectedStatusCode < 300)
            {
                // delete fails
                await client.AddApiKey(apiKey)
                .AddToken(tokens.AccessToken)
                .DeleteAsync(
                    deleteSignIn,
                    await HttpClientExtensions.GetUrl(
                        nameof(AuthController)[..^10],
                        Urn.Delete),
                    HttpStatusCode.NotFound);
            }
        }

        [Theory]
        [InlineData(
            new Role[0],
            Urn.Options,
            Urn.SignIn,
            Urn.SignUp)]
        [InlineData(
            new[] {Role.Accessor},
            Urn.Options,
            Urn.SignIn,
            Urn.SignUp,
            Urn.ChangePassword,
            Urn.Delete)]
        [InlineData(
            new[] {Role.Refresher},
            Urn.Refresh,
            Urn.Options,
            Urn.SignIn,
            Urn.SignUp)]
        public async Task Options(Role[] roles, params Urn[] urns)
        {
            await TestFactory.GetClient()
                .AddApiKey()
                .AddToken(roles)
                .OptionsAsync(
                    _ => HttpClientExtensions.GetUrl(
                        nameof(AuthController)[..^10],
                        Urn.Options),
                    HttpStatusCode.OK,
                    urns,
                    nameof(AuthController)[..^10]);
        }

        [Theory]
        [MemberData(nameof(AuthControllerTests.RefreshTestData))]
        public async Task Refresh(
            string description,
            string apiKey,
            Invitation invitation,
            SignUp signUp,
            HttpStatusCode expectedStatusCode,
            SignIn signIn,
            params object[] other
        )
        {
            Assert.NotNull(other);

            var client = await AuthControllerTests.SignUpTest(
                description,
                TestFactory.ApiKey,
                invitation,
                signUp,
                HttpStatusCode.OK);

            var tokens = await client.PostAsync<SignIn, Token>(
                signIn,
                () => HttpClientExtensions.GetUrl(
                    nameof(AuthController)[..^10],
                    Urn.SignIn),
                HttpStatusCode.OK,
                (_, _) => { });
            Assert.NotNull(tokens);

            // should fail using access token
            await client.AddApiKey(apiKey)
                .AddToken(tokens.AccessToken)
                .PostAsync<Token>(
                    () => HttpClientExtensions.GetUrl(
                        nameof(AuthController)[..^10],
                        Urn.Refresh),
                    HttpStatusCode.Forbidden);

            // should fail without id claim
            await client.AddApiKey(apiKey)
                .AddToken(tokens.RefreshToken)
                .PostAsync<Token>(
                    () => HttpClientExtensions.GetUrl(
                        nameof(AuthController)[..^10],
                        Urn.Refresh),
                    HttpStatusCode.Unauthorized);

            // should fail using refresh token due to not before value
            await client.AddApiKey(apiKey)
                .AddToken(tokens.RefreshToken)
                .PostAsync<Token>(
                    () => HttpClientExtensions.GetUrl(
                        nameof(AuthController)[..^10],
                        Urn.Refresh),
                    HttpStatusCode.Unauthorized);

            // test should pass using new refresh token
            var decoded = ClientJwtTokenService.Decode(tokens.RefreshToken);
            var refreshToken = ClientJwtTokenService.CreateToken(
                decoded.Claims,
                DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow,
                decoded.Issuer,
                decoded.Audiences.First());
            await client.AddApiKey(apiKey)
                .AddToken(refreshToken)
                .PostAsync<Token>(
                    () => HttpClientExtensions.GetUrl(
                        nameof(AuthController)[..^10],
                        Urn.Refresh),
                    HttpStatusCode.OK);

            // refresh should fail reusing token
            await client.AddApiKey(apiKey)
                .AddToken(refreshToken)
                .PostAsync<Token>(
                    () => HttpClientExtensions.GetUrl(
                        nameof(AuthController)[..^10],
                        Urn.Refresh),
                    HttpStatusCode.NotFound);
        }

        [Theory]
        [MemberData(nameof(AuthControllerTests.SignInTestData))]
        public async Task SignIn(
            string description,
            string apiKey,
            Invitation invitation,
            SignUp signUp,
            HttpStatusCode expectedStatusCode,
            SignIn signIn,
            params object[] other
        )
        {
            Assert.NotNull(other);

            HttpClient? client = null;
            if ((int) expectedStatusCode > 199 && (int) expectedStatusCode < 300)
            {
                client = await AuthControllerTests.SignUpTest(
                    description,
                    apiKey,
                    invitation,
                    signUp,
                    expectedStatusCode);
            }

            client ??= TestFactory.GetClient();
            await client.AddApiKey(apiKey)
            .RemoveToken()
            .PostAsync<SignIn, Token>(
                signIn,
                () => HttpClientExtensions.GetUrl(
                    nameof(AuthController)[..^10],
                    Urn.SignIn),
                expectedStatusCode,
                (_, _) => { });

            if ((int) expectedStatusCode < 200 || (int) expectedStatusCode > 299)
            {
                return;
            }

            // sign in fails using different id
            await client.PostAsync<SignIn, Token>(
                new SignIn(
                    signIn.Password,
                    Guid.NewGuid().ToString()),
                () => HttpClientExtensions.GetUrl(
                    nameof(AuthController)[..^10],
                    Urn.SignIn),
                HttpStatusCode.NotFound,
                (_, _) => { });

            // sign in fails using different password
            await client.PostAsync<SignIn, Token>(
                new SignIn(
                    Guid.NewGuid().ToString(),
                    signIn.Id),
                () => HttpClientExtensions.GetUrl(
                    nameof(AuthController)[..^10],
                    Urn.SignIn),
                HttpStatusCode.Unauthorized,
                (_, _) => { });
        }

        [Theory]
        [MemberData(nameof(AuthControllerTests.SignUpTestData))]
        public async Task SignUp(
            string description,
            string apiKey,
            Invitation invitation,
            SignUp signUp,
            HttpStatusCode expectedStatusCode,
            params object[] other
        )
        {
            Assert.NotNull(other);

            await AuthControllerTests.SignUpTest(
                description,
                apiKey,
                invitation,
                signUp,
                expectedStatusCode);
        }

        private static object[] CreateSignUpTestData(
            string description,
            string? apiKey = TestFactory.ApiKey,
            string? invitationId = null,
            Role[]? invitationRoles = null,
            string? signUpDisplayName = null,
            string? signUpInvitationCode = null,
            string? signUpPassword = null,
            string? signUpId = null,
            HttpStatusCode? expectedStatusCode = null,
            string? changePasswordOldPassword = null,
            string? changePasswordNewPassword = null,
            string? deletePassword = null,
            string? deleteId = null
        )
        {
            var fallbackInvitationId = Guid.NewGuid().ToString();
            var fallbackSignUpPassword = Guid.NewGuid().ToString();
            var fallbackSignUpId = Guid.NewGuid().ToString();
            return new object[]
            {
                description,
                apiKey ?? TestFactory.ApiKey,
                new Invitation(
                    invitationId ?? fallbackInvitationId,
                    invitationRoles ??
                    new[]
                    {
                        Role.User,
                        Role.Accessor
                    }),
                new SignUp(
                    signUpDisplayName ?? Guid.NewGuid().ToString(),
                    signUpInvitationCode ?? fallbackInvitationId,
                    signUpPassword ?? fallbackSignUpPassword,
                    signUpId ?? fallbackSignUpId),
                expectedStatusCode ?? HttpStatusCode.OK,
                new SignIn(
                    signUpPassword ?? fallbackSignUpPassword,
                    signUpId ?? fallbackSignUpId),
                new ChangePassword(
                    changePasswordOldPassword ?? fallbackSignUpPassword,
                    changePasswordNewPassword ?? Guid.NewGuid().ToString()),
                new SignIn(
                    deletePassword ?? fallbackSignUpPassword,
                    deleteId ?? fallbackSignUpId)
            };
        }

        private static async Task<Token?> SignUpAsync(
            HttpClient client,
            HttpStatusCode expectedStatusCode,
            SignUp signUp
        )
        {
            var tokens = await client.PostAsync<SignUp, Token>(
                signUp,
                () => HttpClientExtensions.GetUrl(
                    nameof(AuthController)[..^10],
                    Urn.SignUp),
                expectedStatusCode,
                (_, _) => { });
            if ((int) expectedStatusCode > 199 && (int) expectedStatusCode < 300)

            {
                Assert.NotNull(tokens);
            }
            else
            {
                Assert.Null(tokens);
            }

            return tokens;
        }

        private static async Task<HttpClient> SignUpTest(
            string description,
            string apiKey,
            Invitation invitation,
            SignUp signUp,
            HttpStatusCode expectedStatusCode
        )
        {
            Assert.NotNull(description);

            var client = new TestFactory().CreateClient()
            .AddApiKey()
            .AddToken(
                Role.Admin,
                Role.Accessor);

            ResultInvitation? invitationResult = null;
            if (expectedStatusCode != HttpStatusCode.BadRequest)
            {
                // create invitation
                invitationResult = await client.PostAsync<Invitation, ResultInvitation>(
                    invitation,
                    () => HttpClientExtensions.GetUrl(
                        nameof(InvitationController)[..^10],
                        Urn.Create),
                    HttpStatusCode.Created,
                    (_, _) => { });
                Assert.NotNull(invitationResult);
            }

            // sign up
            var tokens = await AuthControllerTests.SignUpAsync(
                client.AddApiKey(apiKey).RemoveToken(),
                expectedStatusCode,
                signUp);

            if ((int) expectedStatusCode < 200 || (int) expectedStatusCode > 299)
            {
                Assert.Null(tokens);
                return client;
            }

            Assert.NotNull(tokens);

            client.AddApiKey()
            .AddToken(
                Role.Admin,
                Role.Accessor);

            // check invitation is deleted
            Assert.NotNull(invitationResult);
            await client.AddApiKey()
                .AddToken(
                    Role.Admin,
                    Role.Accessor)
                .GetAsync<ResultInvitation>(
                    invitationResult.Links.First(
                            link => link.Urn == $"urn:{nameof(InvitationController)[..^10]}:{Urn.ReadById}")
                        .Url,
                    HttpStatusCode.NotFound,
                    _ => { });

            // check refresh token exists

            Assert.NotNull(tokens);
            var tokenUrl = await HttpClientExtensions.GetUrl(
                nameof(TokenEntryController)[..^10],
                Urn.ReadById);
            Assert.NotNull(tokenUrl);

            var decodedToken = ClientJwtTokenService.Decode(tokens.RefreshToken);
            Assert.NotNull(decodedToken);
            var tokenId = decodedToken.Claims.First(claim => claim.Type == Constants.RefreshTokenIdClaimType).Value;

            await client.GetAsync<ResultTokenEntry>(
                $"{tokenUrl}{tokenId}",
                HttpStatusCode.OK,
                token =>
                {
                    Assert.Equal(
                        tokenId,
                        token.Id);
                    Assert.Equal(
                        signUp.Id,
                        token.UserId);
                });

            // check user
            var userUrl = await HttpClientExtensions.GetUrl(
                nameof(UserController)[..^10],
                Urn.ReadById);
            Assert.NotNull(userUrl);

            await client.GetAsync<ResultUser>(
                $"{userUrl}{signUp.Id}",
                HttpStatusCode.OK,
                user =>
                {
                    Assert.Equal(
                        signUp.Id,
                        user.Id);
                    Assert.Equal(
                        signUp.DisplayName,
                        user.DisplayName);
                    Assert.Equal(
                        invitation.Roles,
                        user.Roles);
                });

            // create conflict
            await AuthControllerTests.SignUpAsync(
                client.AddApiKey(apiKey).RemoveToken(),
                HttpStatusCode.Conflict,
                signUp);

            return client;
        }
    }
}
