namespace WebApi.NetCore.Api.Middleware.ErrorHandling
{
    using System.Net;
    using System.Text.Json;
    using WebApi.NetCore.Api.Exceptions;

    /// <summary>
    ///     Middleware for handling errors.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        /// <summary>
        ///     The request delegate.
        /// </summary>
        private readonly RequestDelegate next;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ErrorHandlingMiddleware" /> class.
        /// </summary>
        /// <param name="next">The next delegate.</param>
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        ///     Invokes the middleware.
        /// </summary>
        /// <param name="context">The current context.</param>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (BadRequestException ex)
            {
                await this.SetResponse(
                    context,
                    HttpStatusCode.BadRequest,
                    ex.Message);
            }
            catch (ConflictException ex)
            {
                await this.SetResponse(
                    context,
                    HttpStatusCode.Conflict,
                    ex.Message);
            }
            catch (NotFoundException ex)
            {
                await this.SetResponse(
                    context,
                    HttpStatusCode.NotFound,
                    ex.Message);
            }
            catch (Exception ex)
            {
                await this.SetResponse(
                    context,
                    HttpStatusCode.InternalServerError,
                    ex.Message);
            }
        }

        /// <summary>
        ///     Sets the response.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="message">The message.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        private Task SetResponse(HttpContext context, HttpStatusCode statusCode, string message)
        {
            var exceptionResult = JsonSerializer.Serialize(new {error = message});
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) statusCode;

            return context.Response.WriteAsync(exceptionResult);
        }
    }
}
