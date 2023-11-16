namespace Generic.Base.Api.Middleware.ErrorHandling
{
    using System.Net;
    using Generic.Base.Api.Exceptions;
    using Generic.Base.Api.Extensions;
    using Microsoft.AspNetCore.Http;

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
                await context.SetResponse(
                    HttpStatusCode.BadRequest,
                    ex.Message);
            }
            catch (ConflictException ex)
            {
                await context.SetResponse(
                    HttpStatusCode.Conflict,
                    ex.Message);
            }
            catch (NotFoundException ex)
            {
                await context.SetResponse(
                    HttpStatusCode.NotFound,
                    ex.Message);
            }
            catch (Exception ex)
            {
                await context.SetResponse(
                    HttpStatusCode.InternalServerError,
                    ex.Message);
            }
        }
    }
}
