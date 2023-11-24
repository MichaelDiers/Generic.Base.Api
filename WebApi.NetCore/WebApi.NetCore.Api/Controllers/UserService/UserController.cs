namespace WebApi.NetCore.Api.Controllers.UserService
{
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Controllers;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = nameof(Role.Admin))]
    [Authorize(Roles = nameof(Role.Accessor))]
    [Route("api/[controller]")]
    public class UserController : UserControllerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CrudController{TCreate,TEntry,TUpdate,TResult}" /> class.
        /// </summary>
        /// <param name="domainService">The domain service.</param>
        /// <param name="transformer">Transformer for entries used by controllers.</param>
        public UserController(
            IDomainService<User, User, User> domainService,
            IControllerTransformer<User, ResultUser> transformer
        )
            : base(
                domainService,
                transformer)
        {
        }
    }
}
