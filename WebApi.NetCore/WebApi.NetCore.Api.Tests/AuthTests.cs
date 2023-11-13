namespace WebApi.NetCore.Api.Tests
{
    using System.Net;
    using WebApi.NetCore.Api.Contracts;

    public class IntegrationTests
    {
        [Theory]
        [InlineData(Role.None)]
        [InlineData(Role.User)]
        public async Task RequiresAdminRoleFailDueToMissingRole(Role role)
        {
            var token = await HttpClientHelper.GetTokenAsync(role);

            await HttpClientHelper.PostAsync<object, object>(
                HttpStatusCode.Forbidden,
                $"{Configuration.Url}/requiresAdminRole",
                token);
        }

        [Fact]
        public async Task RequiresAdminRoleFailDueToMissingToken()
        {
            await HttpClientHelper.PostAsync<object, object>(
                HttpStatusCode.Unauthorized,
                $"{Configuration.Url}/requiresAdminRole");
        }

        [Fact]
        public async Task RequiresAdminRoleOk()
        {
            var token = await HttpClientHelper.GetTokenAsync(Role.Admin);

            await HttpClientHelper.PostAsync<object, object>(
                HttpStatusCode.OK,
                $"{Configuration.Url}/requiresAdminRole",
                token);
        }

        [Fact]
        public async Task RequiresAdminRoleOrUserRoleFailDueToMissingRole()
        {
            var token = await HttpClientHelper.GetTokenAsync(Role.None);

            await HttpClientHelper.PostAsync<object, object>(
                HttpStatusCode.Forbidden,
                $"{Configuration.Url}/requiresAdminRoleOrUserRole",
                token);
        }

        [Fact]
        public async Task RequiresAdminRoleOrUserRoleFailDueToMissingToken()
        {
            await HttpClientHelper.PostAsync<object, object>(
                HttpStatusCode.Unauthorized,
                $"{Configuration.Url}/requiresAdminRoleOrUserRole");
        }

        [Theory]
        [InlineData(Role.User)]
        [InlineData(Role.Admin)]
        public async Task RequiresAdminRoleOrUserRoleOk(Role role)
        {
            var token = await HttpClientHelper.GetTokenAsync(role);

            await HttpClientHelper.PostAsync<object, object>(
                HttpStatusCode.OK,
                $"{Configuration.Url}/requiresAdminRoleOrUserRole",
                token);
        }

        [Fact]
        public async Task RequiresAdminRoleOrUserRoleOkMultipleMatch()
        {
            var token = await HttpClientHelper.GetTokenAsync(
                Role.Admin,
                Role.User);

            await HttpClientHelper.PostAsync<object, object>(
                HttpStatusCode.OK,
                $"{Configuration.Url}/requiresAdminRoleOrUserRole",
                token);
        }

        [Fact]
        public async Task RequiresAdminRoleOrUserRoleOkSingleMatch()
        {
            var token = await HttpClientHelper.GetTokenAsync(
                Role.None,
                Role.User);

            await HttpClientHelper.PostAsync<object, object>(
                HttpStatusCode.OK,
                $"{Configuration.Url}/requiresAdminRoleOrUserRole",
                token);
        }

        [Fact]
        public async Task RequiresTokenFailWithInvalidToken()
        {
            await HttpClientHelper.PostAsync<object, object>(
                HttpStatusCode.Unauthorized,
                $"{Configuration.Url}/requiresToken",
                "Token");
        }

        [Fact]
        public async Task RequiresTokenFailWithoutToken()
        {
            await HttpClientHelper.PostAsync<object, object>(
                HttpStatusCode.Unauthorized,
                $"{Configuration.Url}/requiresToken");
        }

        [Fact]
        public async Task RequiresTokenOk()
        {
            var token = await HttpClientHelper.GetTokenAsync(Role.None);

            await HttpClientHelper.PostAsync<object, object>(
                HttpStatusCode.OK,
                $"{Configuration.Url}/requiresToken",
                token);
        }

        [Theory]
        [InlineData(Role.None)]
        [InlineData(Role.Admin)]
        public async Task RequiresUserRoleFailDueToMissingRole(Role role)
        {
            var token = await HttpClientHelper.GetTokenAsync(role);

            await HttpClientHelper.PostAsync<object, object>(
                HttpStatusCode.Forbidden,
                $"{Configuration.Url}/requiresUserRole",
                token);
        }

        [Fact]
        public async Task RequiresUserRoleFailDueToMissingToken()
        {
            await HttpClientHelper.PostAsync<object, object>(
                HttpStatusCode.Unauthorized,
                $"{Configuration.Url}/requiresUserRole");
        }

        [Fact]
        public async Task RequiresUserRoleOk()
        {
            var token = await HttpClientHelper.GetTokenAsync(Role.User);

            await HttpClientHelper.PostAsync<object, object>(
                HttpStatusCode.OK,
                $"{Configuration.Url}/requiresUserRole",
                token);
        }

        [Fact]
        public async Task SignInAsAdmin()
        {
            await HttpClientHelper.GetTokenAsync(Role.Admin);
        }

        [Fact]
        public async Task SignInAsNone()
        {
            await HttpClientHelper.GetTokenAsync(Role.None);
        }

        [Fact]
        public async Task SignInAsUser()
        {
            await HttpClientHelper.GetTokenAsync(Role.User);
        }
    }
}
