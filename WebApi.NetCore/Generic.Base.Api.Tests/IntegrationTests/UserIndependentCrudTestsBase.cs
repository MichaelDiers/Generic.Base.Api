namespace Generic.Base.Api.Tests.IntegrationTests
{
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory;
    using Generic.Base.Api.Tests.Lib.CrudTest;

    public abstract class UserIndependentCrudTestsBase<TCreate, TUpdate, TResult>
        : CrudTests<Program, TestFactory, TCreate, TResult, TResult, TUpdate, TResult>
        where TCreate : class where TResult : class, ILinkResult where TUpdate : class
    {
        protected UserIndependentCrudTestsBase(
            string urnNamespace,
            IEnumerable<Role> requiredCreateRoles,
            IEnumerable<Role> requiredReadAllRoles,
            IEnumerable<Role> requiredReadByIdRoles,
            IEnumerable<Role> requiredUpdateRoles,
            IEnumerable<Role> requiredDeleteRoles
        )
            : base(
                urnNamespace,
                TestFactory.EntryPointUrl,
                TestFactory.ApiKey,
                new[]
                {
                    Role.Accessor,
                    Role.Refresher,
                    Role.Admin,
                    Role.User
                },
                requiredCreateRoles,
                requiredReadAllRoles,
                requiredReadByIdRoles,
                requiredUpdateRoles,
                requiredDeleteRoles)
        {
        }
    }
}
