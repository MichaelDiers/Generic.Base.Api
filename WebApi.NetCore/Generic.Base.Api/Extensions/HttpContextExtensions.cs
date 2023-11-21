namespace Generic.Base.Api.Extensions
{
    using System.Net;
    using Generic.Base.Api.Models;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    ///     Extensions for <see cref="HttpContext" />.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        ///     Sets the response.
        /// </summary>
        /// <param name="context">The http context.</param>
        /// <param name="statusCode">The status code of the response.</param>
        /// <param name="message">The response error message.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public static Task SetResponse(
            this HttpContext context,
            HttpStatusCode statusCode,
            string message,
            CancellationToken cancellationToken = new()
        )
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = (int) statusCode;

            return context.Response.WriteAsJsonAsync(
                new ErrorResult(message),
                typeof(ErrorResult),
                cancellationToken);
        }
    }
}
