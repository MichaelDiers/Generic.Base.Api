namespace Generic.Base.Api.Tests.IntegrationTests
{
    using System.Net;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory;
    using Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.Items;

    public class ItemControllerTests
    {
        private readonly string urnNamespace = nameof(ItemController)[..^10];

        public static IEnumerable<object[]> CreateTestData => new[] {ItemControllerTestsEntry.TestDataForCreate()};

        [Theory]
        [MemberData(nameof(ItemControllerTests.CreateTestData))]
        public void Create(ItemControllerTestsEntry testData)
        {
            //var result = GenericCrudControllerTests.Create(testData.UrnNamespace, testData.Create, testData.ExpectedStatusCode,testData.AssertsForCreate, testData.RolesForCreate, )
        }

        [Theory]
        [InlineData(
            new Role[0],
            new[] {Urn.Options})]
        [InlineData(
            new[] {Role.Admin},
            new[] {Urn.Options})]
        [InlineData(
            new[]
            {
                Role.Admin,
                Role.Accessor
            },
            new[]
            {
                Urn.Options,
                Urn.Create,
                Urn.ReadAll,
                Urn.ReadById
            })]
        public async Task Options(Role[] roles, Urn[] urns)
        {
            await GenericCrudControllerTests.Options(
                this.urnNamespace,
                HttpStatusCode.OK,
                roles,
                urns,
                TestFactory.ApiKey);
        }
    }
}
