namespace Generic.Base.Api.Tests.IntegrationTests
{
    using System.Net;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.Items;

    public class ItemControllerTestsEntry
    {
        public ItemControllerTestsEntry(
            HttpStatusCode expectedStatusCode,
            CreateItem create,
            Action<CreateItem, Item> assertsForCreate,
            Role[] rolesForCreate
        )
        {
            this.Create = create;
            this.AssertsForCreate = assertsForCreate;
            this.RolesForCreate = rolesForCreate;
            this.ExpectedStatusCode = expectedStatusCode;
        }

        public Action<CreateItem, Item> AssertsForCreate { get; }

        public CreateItem Create { get; }

        public HttpStatusCode ExpectedStatusCode { get; }
        public Role[] RolesForCreate { get; }

        public string UrnNamespace => nameof(ItemController)[..^10];

        public static object[] TestDataForCreate(
            HttpStatusCode expectedStatusCode = HttpStatusCode.Created,
            string? nameForCreate = null,
            Action<CreateItem, Item>? assertsForCreate = null,
            Role[]? rolesForCreate = null
        )
        {
            return new object[]
            {
                new ItemControllerTestsEntry(
                    expectedStatusCode,
                    new CreateItem(nameForCreate ?? Guid.NewGuid().ToString()),
                    assertsForCreate ?? new Action<CreateItem, Item>((_, _) => { }),
                    rolesForCreate ??
                    new[]
                    {
                        Role.Accessor,
                        Role.User
                    })
            };
        }
    }
}
