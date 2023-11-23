namespace Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory
{
    using System.Net;
    using Generic.Base.Api.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class SetResponseController : ControllerBase
    {
        [HttpGet("{status:int}/{message}")]
        [AllowAnonymous]
        public async Task Get([FromRoute] int status, [FromRoute] string message, CancellationToken cancellationToken)
        {
            await this.HttpContext.SetResponse(
                (HttpStatusCode) status,
                message,
                cancellationToken);
        }
    }
}
