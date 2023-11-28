namespace Generic.Base.Api.Tests.IntegrationTests.ItemControllerTest
{
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.UserIndependentItems;
    using CreateItem = Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.Items.CreateItem;
    using ResultItem = Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.Items.ResultItem;
    using UpdateItem = Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.Items.UpdateItem;

    public class UserIndependentItemControllerTests : UserIndependentCrudTestsBase<CreateItem, UpdateItem, ResultItem>
    {
        public UserIndependentItemControllerTests()
            : base(
                nameof(Item2Controller)[..^10],
                new[]
                {
                    Role.Accessor,
                    Role.User
                },
                new[]
                {
                    Role.Accessor,
                    Role.User
                },
                new[]
                {
                    Role.Accessor,
                    Role.User
                },
                new[]
                {
                    Role.Accessor,
                    Role.User
                },
                new[]
                {
                    Role.Accessor,
                    Role.User
                })
        {
        }

        protected override bool RaiseDoubleCreateConflict => false;

        protected override CreateItem GetValidCreateEntry()
        {
            return new CreateItem($"CreateName_{Guid.NewGuid()}");
        }

        protected override UpdateItem GetValidUpdateEntry()
        {
            return new UpdateItem($"UpdateName_{Guid.NewGuid()}");
        }
    }
}
