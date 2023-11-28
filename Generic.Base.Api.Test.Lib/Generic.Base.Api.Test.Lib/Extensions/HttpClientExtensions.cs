namespace Generic.Base.Api.Tests.Lib.Extensions
{
    using System.Net;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Security.Claims;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Tests.Lib.CrudTest;

    public static class HttpClientExtensions
    {
        public const string XApiKeyName = "x-api-key";

        public static HttpClient AddApiKey(this HttpClient client, string apiKey)
        {
            client.DefaultRequestHeaders.Remove(HttpClientExtensions.XApiKeyName);
            client.DefaultRequestHeaders.Add(
                HttpClientExtensions.XApiKeyName,
                apiKey);
            return client;
        }

        public static HttpClient AddToken(this HttpClient client, IEnumerable<Role> roles, string? userId = null)
        {
            var claims = roles.Select(
                role => new Claim(
                    ClaimTypes.Role,
                    role.ToString()));
            if (!string.IsNullOrWhiteSpace(userId))
            {
                claims = claims.Append(
                    new Claim(
                        Constants.UserIdClaimType,
                        userId));
            }

            return client.AddToken(claims);
        }

        public static HttpClient AddToken(this HttpClient client, IEnumerable<Claim> claims)
        {
            var token = ClientJwtTokenService.CreateToken(claims);
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {token}");
            return client;
        }

        public static HttpClient Clear(this HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = null;
            client.DefaultRequestHeaders.Remove(HttpClientExtensions.XApiKeyName);
            return client;
        }

        public static async Task DeleteAsync(this HttpClient client, string url, HttpStatusCode expectedStatusCode)
        {
            var response = await client.DeleteAsync(url);

            Assert.Equal(
                expectedStatusCode,
                response.StatusCode);
        }

        public static Task<TResponseData?> GetAsync<TResponseData>(
            this HttpClient client,
            string url,
            HttpStatusCode expectedStatusCode
        ) where TResponseData : class
        {
            return client.SendAsync<TResponseData>(
                HttpMethod.Get,
                url,
                expectedStatusCode);
        }

        public static async Task<TResponseData> OptionsAsync<TResponseData>(this HttpClient client, string url)
            where TResponseData : class
        {
            var result = await client.SendAsync<TResponseData>(
                HttpMethod.Options,
                url,
                HttpStatusCode.OK);

            Assert.NotNull(result);

            return result;
        }

        public static Task<TResponseData?> PostAsync<TRequestData, TResponseData>(
            this HttpClient client,
            string url,
            TRequestData? requestData,
            HttpStatusCode expectedStatusCode
        ) where TResponseData : class
        {
            return client.SendAsync<TRequestData, TResponseData>(
                HttpMethod.Post,
                url,
                requestData,
                expectedStatusCode);
        }

        public static Task<TResponseData?> PutAsync<TRequestData, TResponseData>(
            this HttpClient client,
            string url,
            TRequestData? requestData,
            HttpStatusCode expectedStatusCode
        ) where TResponseData : class
        {
            return client.SendAsync<TRequestData, TResponseData>(
                HttpMethod.Put,
                url,
                requestData,
                expectedStatusCode);
        }

        private static async Task<TResponseData?> SendAsync<TResponseData>(
            this HttpClient client,
            HttpMethod httpMethod,
            string url,
            HttpStatusCode expectedStatusCode
        ) where TResponseData : class
        {
            var response = await client.SendAsync(
                new HttpRequestMessage(
                    httpMethod,
                    url));

            Assert.Equal(
                expectedStatusCode,
                response.StatusCode);

            if ((int) expectedStatusCode < 200 || (int) expectedStatusCode > 299)
            {
                return null;
            }

            var responseData = await response.Content.ReadFromJsonAsync<TResponseData>();
            Assert.NotNull(responseData);

            return responseData;
        }

        private static async Task<TResponseData?> SendAsync<TRequestData, TResponseData>(
            this HttpClient client,
            HttpMethod httpMethod,
            string url,
            TRequestData? requestData,
            HttpStatusCode expectedStatusCode
        ) where TResponseData : class
        {
            var message = new HttpRequestMessage(
                httpMethod,
                url) {Content = JsonContent.Create(requestData)};
            var response = await client.SendAsync(message);

            Assert.Equal(
                expectedStatusCode,
                response.StatusCode);

            if ((int) expectedStatusCode < 199 || (int) expectedStatusCode > 299)
            {
                return null;
            }

            var responseData = await response.Content.ReadFromJsonAsync<TResponseData>();
            Assert.NotNull(responseData);

            return responseData;
        }
    }
}
