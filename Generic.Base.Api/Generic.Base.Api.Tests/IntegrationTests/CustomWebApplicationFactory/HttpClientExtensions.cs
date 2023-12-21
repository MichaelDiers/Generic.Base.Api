namespace Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory
{
    using System.Net;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Text.RegularExpressions;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Models;

    /// <summary>
    ///     Extensions for <see cref="HttpClient" />.
    /// </summary>
    internal static class HttpClientExtensions
    {
        /// <summary>
        ///     Adds the API key.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="apiKey">An optional api key.</param>
        /// <returns>The given <see cref="HttpClient" />.</returns>
        public static HttpClient AddApiKey(this HttpClient client, string? apiKey = TestFactory.ApiKey)
        {
            if (apiKey is null)
            {
                return client;
            }

            client.DefaultRequestHeaders.Remove("x-api-key");
            client.DefaultRequestHeaders.Add(
                "x-api-key",
                apiKey);

            return client;
        }

        /// <summary>
        ///     Adds a json web token.
        /// </summary>
        /// <param name="client">The http client.</param>
        /// <param name="roles">The roles to be added.</param>
        /// <returns>The given <paramref name="client" />.</returns>
        public static HttpClient AddToken(this HttpClient client, params Role[] roles)
        {
            var token = ClientJwtTokenService.CreateToken(roles);
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {token}");
            return client;
        }

        /// <summary>
        ///     Adds a json web token.
        /// </summary>
        /// <param name="client">The http client.</param>
        /// <param name="token">The token added to authorization header.</param>
        /// <returns>The given <paramref name="client" />.</returns>
        public static HttpClient AddToken(this HttpClient client, string token)
        {
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {token}");
            return client;
        }

        /// <summary>
        ///     Executes the <see cref="HttpMethod.Delete" /> operation.
        /// </summary>
        /// <param name="client">The http client.</param>
        /// <param name="url">The url of the operation.</param>
        /// <param name="statusCode">The expected status code.</param>
        public static async Task DeleteAsync(this HttpClient client, string url, HttpStatusCode statusCode)
        {
            var response = await client.DeleteAsync(url);
            Assert.Equal(
                statusCode,
                response.StatusCode);
        }

        /// <summary>
        ///     Executes the <see cref="HttpMethod.Delete" /> operation.
        /// </summary>
        /// <typeparam name="T">The type of the request data.</typeparam>
        /// <param name="client">The http client.</param>
        /// <param name="request">The request data.</param>
        /// <param name="url">The url of the operation.</param>
        /// <param name="statusCode">The expected status code.</param>
        public static async Task DeleteAsync<T>(
            this HttpClient client,
            T request,
            string url,
            HttpStatusCode statusCode
        )
        {
            var message = new HttpRequestMessage(
                HttpMethod.Delete,
                url);
            message.Content = JsonContent.Create(request);
            var response = await client.SendAsync(message);
            Assert.Equal(
                statusCode,
                response.StatusCode);
        }

        /// <summary>
        ///     Executes the <see cref="HttpMethod.Get" /> operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="client">The http client.</param>
        /// <param name="url">The url of the operation.</param>
        /// <param name="statusCode">The expected status code.</param>
        /// <param name="asserts">The asserts for checking the result.</param>
        /// <returns>The result of the operation.</returns>
        public static async Task<TResponse?> GetAsync<TResponse>(
            this HttpClient client,
            string url,
            HttpStatusCode statusCode,
            Action<TResponse> asserts
        ) where TResponse : class
        {
            var response = await client.GetAsync(url);
            Assert.Equal(
                statusCode,
                response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<TResponse>();
            Assert.NotNull(result);

            asserts(result);

            return result;
        }

        /// <summary>
        ///     Gets the url specified by the urn namespace and urn operation.
        /// </summary>
        /// <param name="client">The http client.</param>
        /// <param name="urnNamespace">The urn namespace.</param>
        /// <param name="urn">The urn operation.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the found url.</returns>
        public static async Task<string> GetUrl(this HttpClient client, string urnNamespace, Urn urn)
        {
            var optionsUrl = await HttpClientExtensions.GetUrl(
                client,
                TestFactory.EntryPointUrl,
                urnNamespace,
                Urn.Options);

            var operationUrl = await HttpClientExtensions.GetUrl(
                client,
                optionsUrl,
                urnNamespace,
                urn);

            return operationUrl;
        }

        /// <summary>
        ///     Gets the url specified by the urn namespace and urn operation.
        /// </summary>
        /// <param name="urnNamespace">The urn namespace.</param>
        /// <param name="urn">The urn operation.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the found url.</returns>
        public static async Task<string> GetUrl(string urnNamespace, Urn urn)
        {
            var client = TestFactory.GetClient()
            .AddApiKey()
            .AddToken(
                Role.Admin,
                Role.Accessor,
                Role.Refresher);
            var optionsUrl = await HttpClientExtensions.GetUrl(
                client,
                TestFactory.EntryPointUrl,
                urnNamespace,
                Urn.Options);

            var operationUrl = await HttpClientExtensions.GetUrl(
                client,
                optionsUrl,
                urnNamespace,
                urn);

            return operationUrl;
        }

        /// <summary>
        ///     Executes the <see cref="HttpMethod.Options" /> operation.
        /// </summary>
        /// <param name="client">The http client.</param>
        /// <param name="getUrl">A <see cref="Task{T}" /> whose result is the options url.</param>
        /// <param name="statusCode">The expected status code.</param>
        /// <param name="urns">The expected urns to be found.</param>
        /// <param name="urnNamespace">The urn namespace.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is a <see cref="LinkResult" />.</returns>
        public static async Task<LinkResult> OptionsAsync(
            this HttpClient client,
            Func<HttpClient, Task<string>> getUrl,
            HttpStatusCode statusCode,
            IEnumerable<Urn> urns,
            string urnNamespace
        )
        {
            var url = await getUrl(client);

            var response = await client.SendAsync(
                new HttpRequestMessage(
                    HttpMethod.Options,
                    url));

            Assert.Equal(
                statusCode,
                response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<LinkResult>();
            Assert.NotNull(result);

            var urnArray = urns.ToArray();
            Assert.Equal(
                urnArray.Length,
                result.Links.Count());

            foreach (var urn in urnArray)
            {
                Assert.Contains(
                    result.Links,
                    link => Regex.IsMatch(
                        link.Urn,
                        $"^urn:{urnNamespace}:{urn.ToString()}"));
            }

            return result;
        }

        /// <summary>
        ///     Executes the <see cref="HttpMethod.Post" /> operation.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="client">The http client.</param>
        /// <param name="request">The data of the post request.</param>
        /// <param name="urlFunc">A <see cref="Task{T}" /> whose result is the post url.</param>
        /// <param name="statusCode">The expected status code.</param>
        /// <param name="asserts">The asserts.</param>
        /// <returns>A <see cref="Task" /> whose result is the response of the post request.</returns>
        public static async Task<TResponse?> PostAsync<TRequest, TResponse>(
            this HttpClient client,
            TRequest request,
            Func<Task<string>> urlFunc,
            HttpStatusCode statusCode,
            Action<TRequest, TResponse> asserts
        ) where TResponse : class
        {
            var url = await urlFunc();

            var response = await client.PostAsync(
                url,
                JsonContent.Create(request));
            Assert.Equal(
                statusCode,
                response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<TResponse>();
            Assert.NotNull(result);

            asserts(
                request,
                result);

            return result;
        }

        /// <summary>
        ///     Executes the <see cref="HttpMethod.Post" /> operation.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="client">The http client.</param>
        /// <param name="urlFunc">A <see cref="Task{T}" /> whose result is the post url.</param>
        /// <param name="statusCode">The expected status code.</param>
        /// <returns>A <see cref="Task" /> whose result is the response of the post request.</returns>
        public static async Task<TResponse?> PostAsync<TResponse>(
            this HttpClient client,
            Func<Task<string>> urlFunc,
            HttpStatusCode statusCode
        ) where TResponse : class
        {
            var url = await urlFunc();

            var response = await client.SendAsync(
                new HttpRequestMessage(
                    HttpMethod.Post,
                    url));
            Assert.Equal(
                statusCode,
                response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<TResponse>();
            Assert.NotNull(result);

            return result;
        }

        /// <summary>
        ///     Executes the <see cref="HttpMethod.Put" /> operation.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="client">The http client.</param>
        /// <param name="request">The data of the put request.</param>
        /// <param name="url">The url used for the request.</param>
        /// <param name="statusCode">The expected status code.</param>
        public static async Task PutAsync<TRequest>(
            this HttpClient client,
            TRequest request,
            string url,
            HttpStatusCode statusCode
        )
        {
            var response = await client.PutAsync(
                url,
                JsonContent.Create(request));
            Assert.Equal(
                statusCode,
                response.StatusCode);
        }

        /// <summary>
        ///     Removes a json web token.
        /// </summary>
        /// <param name="client">The http client.</param>
        /// <returns>The given <paramref name="client" />.</returns>
        public static HttpClient RemoveToken(this HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = null;
            return client;
        }

        /// <summary>
        ///     Gets the url for the given urn namespace and operation.
        /// </summary>
        /// <param name="client">The http client.</param>
        /// <param name="url">The url of the operation.</param>
        /// <param name="urnNamespace">The urn namespace.</param>
        /// <param name="urn">The urn of the operation.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the url.</returns>
        private static async Task<string> GetUrl(
            HttpClient client,
            string url,
            string urnNamespace,
            Urn urn
        )
        {
            var optionsResponse = await client.SendAsync(
                new HttpRequestMessage(
                    HttpMethod.Options,
                    url));
            optionsResponse.EnsureSuccessStatusCode();

            var linkResult = await optionsResponse.Content.ReadFromJsonAsync<LinkResult>();
            Assert.NotNull(linkResult);

            var link = linkResult.Links.FirstOrDefault(
                link => Regex.IsMatch(
                    link.Urn,
                    $"^urn:{urnNamespace}:{urn.ToString()}"));
            Assert.NotNull(link);

            return link.Url;
        }
    }
}
