namespace Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.UserIndependentItems
{
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Controllers;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Authorize(Roles = nameof(Role.User))]
    [Authorize(Roles = nameof(Role.Accessor))]
    [Route("api/[controller]")]
    public class Item2Controller : CrudController<CreateItem, Item, UpdateItem, ResultItem>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CrudController{TCreate, TEntry, TUpdate, TResult}" /> class.
        /// </summary>
        /// <param name="domainService">The domain service.</param>
        /// <param name="transformer">Transformer for entries used by controllers.</param>
        public Item2Controller(
            IDomainService<CreateItem, Item, UpdateItem> domainService,
            IControllerTransformer<Item, ResultItem> transformer
        )
            : base(
                domainService,
                transformer)
        {
        }

        /// <summary>
        ///     Determines whether the specified identifier is valid.
        /// </summary>
        /// <param name="id">The identifier to be checked.</param>
        /// <returns>
        ///     <c>true</c> if the specified identifier is valid; otherwise, <c>false</c>.
        /// </returns>
        protected override bool IsIdValid(string id)
        {
            return true;
        }
    }
}
