namespace Generic.Base.Api.Tests.IntegrationTests.ItemControllerTest
{
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.Items;

    public class UserBoundItemControllerTests : UserBoundCrudTestsBase<CreateItem, UpdateItem, ResultItem>
    {
        public UserBoundItemControllerTests()
            : base(
                nameof(ItemController)[..^10],
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
