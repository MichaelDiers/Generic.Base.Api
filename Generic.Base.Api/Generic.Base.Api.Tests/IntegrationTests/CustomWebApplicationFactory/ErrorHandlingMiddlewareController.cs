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
            throw exception switch
            {
                "BadRequestException" => new BadRequestException(),
                "ConflictException" => new ConflictException(),
                "NotFoundException" => new NotFoundException(),
                "UnauthorizedException" => new UnauthorizedException(),
                "ArgumentException" => new ArgumentException(),
                _ => new Exception()
            };
        }
    }
}
