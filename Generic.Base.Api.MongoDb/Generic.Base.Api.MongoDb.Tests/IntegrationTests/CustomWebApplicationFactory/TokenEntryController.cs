namespace Generic.Base.Api.MongoDb.Tests.IntegrationTests.CustomWebApplicationFactory
{
    using System.Security.Claims;
    using Generic.Base.Api.AuthServices.TokenService;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    ///     Test controller for <see cref="TokenEntryControllerBase" />.
    /// </summary>
    /// <seealso cref="Generic.Base.Api.AuthServices.TokenService.TokenEntryControllerBase" />
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = nameof(Role.Admin))]
    [Authorize(Roles = nameof(Role.Accessor))]
    public class TokenEntryController : TokenEntryControllerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TokenEntryControllerBase" /> class.
        /// </summary>
        /// <param name="domainService">The domain service.</param>
        /// <param name="transformer">Transformer for entries used by controllers.</param>
        public TokenEntryController(
            IDomainService<TokenEntry, TokenEntry, TokenEntry> domainService,
            IControllerTransformer<TokenEntry, ResultTokenEntry> transformer
        )
            : base(
                domainService,
                transformer,
                new[]
                {
                    new Claim(
                        ClaimTypes.Role,
                        Role.Admin.ToString()),
                    new Claim(
                        ClaimTypes.Role,
                        Role.Accessor.ToString())
                })
        {
        }
    }
}
