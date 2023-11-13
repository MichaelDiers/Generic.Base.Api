namespace WebApi.NetCore.Api.Middleware
{
    using System.Net;
    using System.Text.Json;

    /// <summary>
    ///     Api key validation middleware.
    /// </summary>
    internal class ApiKeyMiddleware
    {
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
        public Task Invoke(HttpContext context)
        {
            var header = context.Request.Headers["x-api-key"].ToString();
            if (string.IsNullOrWhiteSpace(header))
            {
                return ApiKeyMiddleware.SetResponse(
                    context,
                    HttpStatusCode.Unauthorized,
                    "Missing api key.");
            }

            if (header != "foobar")
            {
                return ApiKeyMiddleware.SetResponse(
                    context,
                    HttpStatusCode.Forbidden,
                    "Invalid api key.");
            }

            return this.next(context);
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
