namespace WebApi.NetCore.Api.Items
{
    using Generic.Base.Api.Controllers;
    using Generic.Base.Api.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : CrudController<CreateItem, Item, UpdateItem, ItemResult>
    {
        public ItemController(IDomainService<CreateItem, Item, UpdateItem> domainService, ItemTransformer transformer)
            : base(
                domainService,
                transformer)
        {
        }
    }
}
