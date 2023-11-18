namespace Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory
{
    using Generic.Base.Api.Controllers;
    using Generic.Base.Api.Models;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    ///     Entry point of the application that provides the links to its controllers via options request.
    /// </summary>
    /// <seealso cref="Generic.Base.Api.Controllers.OptionsControllerBase" />
    [ApiController]
    [Route("api/[Controller]")]
    public class OptionsController : OptionsControllerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="OptionsController" /> class.
        /// </summary>
        public OptionsController()
            : base(
                new ClaimLink(
                    Urn.Options,
                    "api/Options"))
        {
        }
    }
}
