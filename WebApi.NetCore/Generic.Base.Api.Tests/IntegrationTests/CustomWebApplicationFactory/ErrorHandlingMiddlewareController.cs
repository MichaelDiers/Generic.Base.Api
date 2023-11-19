namespace Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory
{
    using Generic.Base.Api.Exceptions;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    ///     Controller for error handling tests.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("api/[controller]")]
    public class ErrorHandlingMiddlewareController : ControllerBase
    {
        [HttpGet("{exception}")]
        public ActionResult Get([FromRoute] string exception)
        {
            switch (exception)
            {
                case "BadRequestException":
                    throw new BadRequestException();
                case "ConflictException":
                    throw new ConflictException();
                case "NotFoundException":
                    throw new NotFoundException();
                case "UnauthorizedException":
                    throw new UnauthorizedException();
                case "ArgumentException":
                    throw new ArgumentException();
                default:
                    throw new Exception();
            }
        }
    }
}
