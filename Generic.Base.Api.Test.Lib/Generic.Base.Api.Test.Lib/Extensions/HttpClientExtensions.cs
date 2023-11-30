namespace Generic.Base.Api.Test.Lib.Extensions
{
    using System.Net;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Security.Claims;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Test.Lib.CrudTest;

    /// <summary>
    ///     Extensions for <see cref="HttpClient" />.
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        ///     The name of the api key header.
        /// </summary>
        public const string XApiKeyName = "x-api-key";

        /// <summary>
        ///     Adds the API key.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="apiKey">The API key.</param>
        /// <returns>The given <paramref name="client" />.</returns>
        public static HttpClient AddApiKey(this HttpClient client, string apiKey)
        {
            client.DefaultRequestHeaders.Remove(HttpClientExtensions.XApiKeyName);
            client.DefaultRequestHeaders.Add(
                HttpClientExtensions.XApiKeyName,
                apiKey);
            return client;
        }

        /// <summary>
        ///     Adds the token.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="roles">The roles.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The given <paramref name="client" />.</returns>
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

        /// <summary>
        ///     Adds the token.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="claims">The claims.</param>
        /// <returns>The given <paramref name="client" />.</returns>
        public static HttpClient AddToken(this HttpClient client, IEnumerable<Claim> claims)
        {
            var token = ClientJwtTokenService.CreateToken(claims);
            return client.AddToken(token);
        }

        /// <summary>
        ///     Adds the token.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="token">The token.</param>
        /// <returns>The given <paramref name="client" />.</returns>
        public static HttpClient AddToken(this HttpClient client, string token)
        {
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {token}");
            return client;
        }

        /// <summary>
        ///     Remove api key and authorization settings from the given client.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <returns>The given <paramref name="client" />.</returns>
        public static HttpClient Clear(this HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = null;
            client.DefaultRequestHeaders.Remove(HttpClientExtensions.XApiKeyName);
            return client;
        }

        /// <summary>
        ///     Sends a delete request.
        /// </summary>
        /// <param name="client">The http client.</param>
        /// <param name="url">The URL.</param>
        /// <param name="expectedStatusCode">The expected status code.</param>
        public static async Task DeleteAsync(this HttpClient client, string url, HttpStatusCode expectedStatusCode)
        {
            var response = await client.DeleteAsync(url);

            await HttpClientExtensions.AssertStatusCode(
                response,
                expectedStatusCode);
        }

        /// <summary>
        ///     Sends a get request.
        /// </summary>
        /// <typeparam name="TResponseData">The type of the response data.</typeparam>
        /// <param name="client">The client.</param>
        /// <param name="url">The URL.</param>
        /// <param name="expectedStatusCode">The expected status code.</param>
        /// <returns>The parsed json response.</returns>
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

        /// <summary>
        ///     Sends an options request.
        /// </summary>
        /// <typeparam name="TResponseData">The type of the response data.</typeparam>
        /// <param name="client">The http client.</param>
        /// <param name="url">The URL.</param>
        /// <returns>The parsed json response.</returns>
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

        /// <summary>
        ///     Sends a post request.
        /// </summary>
        /// <typeparam name="TRequestData">The type of the request data.</typeparam>
        /// <typeparam name="TResponseData">The type of the response data.</typeparam>
        /// <param name="client">The http client.</param>
        /// <param name="url">The URL.</param>
        /// <param name="requestData">The request data.</param>
        /// <param name="expectedStatusCode">The expected status code.</param>
        /// <returns>The parsed json response.</returns>
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

        /// <summary>
        ///     Sends a put request.
        /// </summary>
        /// <typeparam name="TRequestData">The type of the request data.</typeparam>
        /// <typeparam name="TResponseData">The type of the response data.</typeparam>
        /// <param name="client">The http client.</param>
        /// <param name="url">The URL.</param>
        /// <param name="requestData">The request data.</param>
        /// <param name="expectedStatusCode">The expected status code.</param>
        /// <returns>The parsed json response.</returns>
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

        /// <summary>
        ///     Asserts the status code.
        /// </summary>
        /// <param name="message">The http message.</param>
        /// <param name="expectedStatusCode">The expected status code.</param>
        private static async Task AssertStatusCode(HttpResponseMessage message, HttpStatusCode expectedStatusCode)
        {
            if (message.StatusCode != expectedStatusCode &&
                ((int) message.StatusCode < 200 || (int) message.StatusCode > 299))
            {
                try
                {
                    var error = await message.Content.ReadFromJsonAsync<ErrorResult>();
                    Assert.Fail(
                        $"Expected status code: {expectedStatusCode}; actual: {message.StatusCode}; message: {error?.Error}");
                }
                catch
                {
                    Assert.Fail($"Expected status code: {expectedStatusCode}; actual: {message.StatusCode}");
                }
            }

            Assert.Equal(
                expectedStatusCode,
                message.StatusCode);
        }

        /// <summary>
        ///     Sends a http request.
        /// </summary>
        /// <typeparam name="TResponseData">The type of the response data.</typeparam>
        /// <param name="client">The http client.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="expectedStatusCode">The expected status code.</param>
        /// <returns>The parsed json response.</returns>
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

            await HttpClientExtensions.AssertStatusCode(
                response,
                expectedStatusCode);

            if ((int) expectedStatusCode < 200 || (int) expectedStatusCode > 299)
            {
                return null;
            }

            var responseData = await response.Content.ReadFromJsonAsync<TResponseData>();
            Assert.NotNull(responseData);

            return responseData;
        }

        /// <summary>
        ///     Sends a http request.
        /// </summary>
        /// <typeparam name="TRequestData">The type of the request data.</typeparam>
        /// <typeparam name="TResponseData">The type of the response data.</typeparam>
        /// <param name="client">The client.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="requestData">The request data.</param>
        /// <param name="expectedStatusCode">The expected status code.</param>
        /// <returns>The parsed json response.</returns>
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

            await HttpClientExtensions.AssertStatusCode(
                response,
                expectedStatusCode);

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
