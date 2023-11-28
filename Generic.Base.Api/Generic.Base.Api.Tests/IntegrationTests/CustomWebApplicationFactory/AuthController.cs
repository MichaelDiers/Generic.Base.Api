namespace Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory
{
    using Generic.Base.Api.AuthServices.AuthService;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    ///     Provides auth operations, like sign in and sign up.
    /// </summary>
    /// <seealso cref="AuthControllerBase" />
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : AuthControllerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthControllerBase" /> class.
        /// </summary>
        /// <param name="domainAuthService">The domain authentication service.</param>
        public AuthController(IDomainAuthService domainAuthService)
            : base(domainAuthService)
        {
        }
    }
}
