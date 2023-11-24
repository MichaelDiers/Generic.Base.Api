namespace WebApi.NetCore.Api.Controllers.TokenEntryService
{
    using Generic.Base.Api.AuthServices.TokenService;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = nameof(Role.Admin))]
    [Authorize(Roles = nameof(Role.Accessor))]
    [Route("api/[controller]")]
    public class TokenEntryController : TokenEntryControllerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TokenEntryController" /> class.
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
