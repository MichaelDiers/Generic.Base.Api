namespace Generic.Base.Api.MongoDb.Tests.IntegrationTests.CustomWebApplicationFactory
{
    using System.Security.Claims;
    using Generic.Base.Api.AuthServices.InvitationService;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Controllers;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = nameof(Role.Admin))]
    [Authorize(Roles = nameof(Role.Accessor))]
    public class InvitationController : InvitationControllerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CrudController{TCreate,TEntry,TUpdate,TResult}" /> class.
        /// </summary>
        /// <param name="domainService">The domain service.</param>
        /// <param name="transformer">Transformer for entries used by controllers.</param>
        public InvitationController(
            IDomainService<Invitation, Invitation, Invitation> domainService,
            IControllerTransformer<Invitation, ResultInvitation> transformer
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
