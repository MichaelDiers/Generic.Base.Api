namespace Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory
{
    using Generic.Base.Api.AuthServices.TokenService;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    ///     Test controller for <see cref="TokenEntryControllerBase" />.
    /// </summary>
    /// <seealso cref="Generic.Base.Api.AuthServices.TokenService.TokenEntryControllerBase" />
    [ApiController]
    [Route("api/[controller]")]
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
                transformer)
        {
        }
    }
}
