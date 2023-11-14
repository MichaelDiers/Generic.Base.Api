namespace WebApi.NetCore.Api.Middleware
{
    using System.Net;
    using System.Text.Json;
    using WebApi.NetCore.Api.Contracts.Configuration;

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
        /// <param name="context">The current context.</param>
        /// <param name="envConfiguration">The environment configuration.</param>
        public Task Invoke(HttpContext context, IEnvConfiguration envConfiguration)
        {
            var apiKey = this.ReadApiKey(context);

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return ApiKeyMiddleware.SetResponse(
                    context,
                    HttpStatusCode.Unauthorized,
                    "Missing api key.");
            }

            if (apiKey != envConfiguration.ApiKey)
            {
                return ApiKeyMiddleware.SetResponse(
                    context,
                    HttpStatusCode.Forbidden,
                    "Invalid api key.");
            }

            return this.next(context);
        }

        /// <summary>
        ///     Reads the API key.
        /// </summary>
        /// <param name="context">The current http context.</param>
        /// <returns>The found api key or an empty string if no api key is found.</returns>
        private string ReadApiKey(HttpContext context)
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

        /// <summary>
        ///     Sets the response.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="message">The message.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        private static Task SetResponse(HttpContext context, HttpStatusCode statusCode, string message)
        {
            var exceptionResult = JsonSerializer.Serialize(new {error = message});
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) statusCode;

            return context.Response.WriteAsync(exceptionResult);
        }
    }
}
