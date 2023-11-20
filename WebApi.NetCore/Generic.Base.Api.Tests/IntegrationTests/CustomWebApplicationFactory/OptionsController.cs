namespace Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory
{
    using System.Security.Claims;
    using Generic.Base.Api.AuthServices.UserService;
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
                    string.Empty),
                new ClaimLink(
                    Urn.Options,
                    $"../{nameof(TokenEntryController)[..^10]}",
                    new[]
                    {
                        new Claim(
                            ClaimTypes.Role,
                            nameof(Role.Admin))
                    }))
        {
        }
    }
}
