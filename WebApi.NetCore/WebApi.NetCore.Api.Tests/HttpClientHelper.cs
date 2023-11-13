namespace WebApi.NetCore.Api.Tests
{
    using System.Net;
    using System.Net.Http.Headers;
    using System.Text;
    using Newtonsoft.Json;
    using WebApi.NetCore.Api.Contracts;

    internal static class HttpClientHelper
    {
        public static async Task<string> GetTokenAsync(params Role[] roles)
        {
            var route = string.Join(
                "/",
                roles.Select(role => (int) role));
            var token = await HttpClientHelper.PostAsync<object, string>(
                HttpStatusCode.OK,
                $"{Configuration.Url}/signInAs/{route}",
                hasResponse: true);
            Assert.NotNull(token);
            return token;
        }

        public static async Task<TResponse?> PostAsync<TRequest, TResponse>(
            HttpStatusCode statusCode,
            string url = Configuration.Url,
            string? token = null,
            TRequest? data = null,
            bool hasResponse = false,
            bool checkLocationHeaderNonEmpty = false,
            bool useApiKey = true,
            string? apiKey = Configuration.ApiKey
        ) where TResponse : class where TRequest : class
        {
            using var httpClient = new HttpClient();

            if (token is not null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token);
            }

            if (useApiKey)
            {
                httpClient.DefaultRequestHeaders.Add(
                    Configuration.ApiKeyHeader,
                    apiKey);
            }

            StringContent? content = null;
            if (data is not null)
            {
                content = new StringContent(
                    JsonConvert.SerializeObject(data),
                    Encoding.UTF8,
                    "application/json");
            }

            var response = await httpClient.PostAsync(
                url,
                content);

            Assert.Equal(
                statusCode,
                response.StatusCode);

            if (checkLocationHeaderNonEmpty)
            {
                Assert.NotNull(response.Headers.Location);
            }

            if (!hasResponse)
            {
                return null;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            if (typeof(TResponse) == typeof(string))
            {
                return jsonResponse as TResponse;
            }

            var responseData = JsonConvert.DeserializeObject<TResponse>(jsonResponse);

            Assert.NotNull(responseData);

            return responseData;
        }
    }
}
