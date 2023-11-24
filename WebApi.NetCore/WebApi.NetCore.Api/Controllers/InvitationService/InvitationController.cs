namespace WebApi.NetCore.Api.Controllers.InvitationService
{
    using Generic.Base.Api.AuthServices.InvitationService;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Controllers;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = nameof(Role.Admin))]
    [Authorize(Roles = nameof(Role.Accessor))]
    [Route("api/[controller]")]
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
                transformer)
        {
        }
    }
}
