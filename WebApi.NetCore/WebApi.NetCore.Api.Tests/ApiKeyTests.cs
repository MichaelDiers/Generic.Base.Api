namespace WebApi.NetCore.Api.Tests
{
    using System.Net;
    using WebApi.NetCore.Api.Contracts;

    public class ApiKeyTests
    {
        [Fact]
        public async Task ApiKeyAdminFailsDueToInvalidApiKey()
        {
            var token = await HttpClientHelper.GetTokenAsync(Role.Admin);

            await HttpClientHelper.PostAsync<object, object>(
                HttpStatusCode.Forbidden,
                $"{Configuration.Url}/apiKeyAdmin",
                token,
                apiKey: Guid.NewGuid().ToString());
        }

        [Fact]
        public async Task ApiKeyAdminFailsDueToMissingApiKey()
        {
            var token = await HttpClientHelper.GetTokenAsync(Role.Admin);

            await HttpClientHelper.PostAsync<object, object>(
                HttpStatusCode.Unauthorized,
                $"{Configuration.Url}/apiKeyAdmin",
                token,
                useApiKey: false);
        }

        [Fact]
        public async Task ApiKeyAdminOk()
        {
            var token = await HttpClientHelper.GetTokenAsync(Role.Admin);

            await HttpClientHelper.PostAsync<object, object>(
                HttpStatusCode.OK,
                $"{Configuration.Url}/apiKeyAdmin",
                token,
                apiKey: Configuration.ApiKey);
        }

        [Fact]
        public async Task ApiKeyAllowAnonymousFailsDueToInvalidApiKey()
        {
            var token = await HttpClientHelper.GetTokenAsync(Role.Admin);

            await HttpClientHelper.PostAsync<object, object>(
                HttpStatusCode.Forbidden,
                $"{Configuration.Url}/apiKeyAllowAnonymous",
                token,
                apiKey: Guid.NewGuid().ToString());
        }

        [Fact]
        public async Task ApiKeyAllowAnonymousFailsDueToMissingApiKey()
        {
            var token = await HttpClientHelper.GetTokenAsync(Role.Admin);

            await HttpClientHelper.PostAsync<object, object>(
                HttpStatusCode.Unauthorized,
                $"{Configuration.Url}/apiKeyAllowAnonymous",
                token,
                useApiKey: false);
        }

        [Fact]
        public async Task ApiKeyAllowAnonymousOk()
        {
            var token = await HttpClientHelper.GetTokenAsync(Role.Admin);

            await HttpClientHelper.PostAsync<object, object>(
                HttpStatusCode.OK,
                $"{Configuration.Url}/apiKeyAllowAnonymous",
                token,
                apiKey: Configuration.ApiKey);
        }
    }
}
