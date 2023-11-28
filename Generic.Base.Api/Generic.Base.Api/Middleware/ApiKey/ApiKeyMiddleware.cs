namespace Generic.Base.Api.Middleware.ApiKey
{
    using System.Net;
    using Generic.Base.Api.Extensions;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    ///     Api key validation middleware.
    /// </summary>
    internal class ApiKeyMiddleware
    {
        /// <summary>
        ///     The name of the header that contains the api key.
        /// </summary>
        private const string ApiKeyHeaderName = "x-api-key";

        /// <summary>
        ///     The request delegate.
        /// </summary>
        private readonly RequestDelegate next;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApiKeyMiddleware" /> class.
        /// </summary>
        /// <param name="next">The next delegate.</param>
        public ApiKeyMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        ///     Invokes the middleware.
        /// </summary>
        /// <param name="context">The current http context.</param>
        /// <param name="configuration">The api key configuration.</param>
        // ReSharper disable once UnusedMember.Global
        public Task Invoke(HttpContext context, IApiKeyConfiguration configuration)
        {
            var apiKey = ApiKeyMiddleware.ReadApiKey(context);

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return context.SetResponse(
                    HttpStatusCode.Unauthorized,
                    "Missing api key.");
            }

            if (apiKey != configuration.ApiKey)
            {
                return context.SetResponse(
                    HttpStatusCode.Forbidden,
                    "Invalid api key.");
            }

            return this.next(context);
        }

        /// <summary>
        ///     Reads the API key from header or query parameter.
        /// </summary>
        /// <param name="context">The current http context.</param>
        /// <returns>The found api key or an empty string if no api key is found.</returns>
        private static string ReadApiKey(HttpContext context)
        {
            var header = context.Request.Headers[ApiKeyMiddleware.ApiKeyHeaderName].ToString();
            if (!string.IsNullOrWhiteSpace(header))
            {
                return header;
            }

            return context.Request.Query.TryGetValue(
                ApiKeyMiddleware.ApiKeyHeaderName,
                out var apiKey)
                ? apiKey.ToString()
                : string.Empty;
        }
    }
}
