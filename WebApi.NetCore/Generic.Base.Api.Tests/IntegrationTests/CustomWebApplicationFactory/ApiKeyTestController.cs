namespace Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory
{
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    ///     Controller for api key tests.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [ApiController]
    [Route("api/[controller]")]
    public class ApiKeyTestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return this.Ok();
        }
    }
}
