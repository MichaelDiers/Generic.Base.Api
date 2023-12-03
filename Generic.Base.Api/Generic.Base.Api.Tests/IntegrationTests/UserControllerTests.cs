namespace Generic.Base.Api.Tests.IntegrationTests
{
    using System.Net;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory;

    /// <summary>
    ///     Tests for <see cref="UserController" />.
    /// </summary>
    [Trait(
        "TestType",
        "InMemoryIntegrationTest")]
    public class UserControllerTests
    {
        /// <summary>
        ///     The default urn namespace for operations on <see cref="User" />.
        /// </summary>
        private readonly string urnNamespace = nameof(UserController)[..^10];

        /// <summary>
        ///     Gets the test data for the <see cref="Create" /> test.
        /// </summary>
        public static IEnumerable<object[]> TestDataForCreate =>
            new[]
            {
                // default test should pass
                UserControllerTests.TestDataEntryForCreate(),
                // api key errors
                UserControllerTests.TestDataEntryForCreate(
                    apiKey: "",
                    expectedStatusCode: HttpStatusCode.Unauthorized),
                UserControllerTests.TestDataEntryForCreate(
                    apiKey: Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // missing roles
                UserControllerTests.TestDataEntryForCreate(
                    roles: new Role[] { },
                    expectedStatusCode: HttpStatusCode.Forbidden),
                UserControllerTests.TestDataEntryForCreate(
                    roles: new[] {Role.User},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                UserControllerTests.TestDataEntryForCreate(
                    roles: new[] {Role.Admin},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                UserControllerTests.TestDataEntryForCreate(
                    roles: new[] {Role.Accessor},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // invalid id
                UserControllerTests.TestDataEntryForCreate(
                    "a",
                    expectedStatusCode: HttpStatusCode.BadRequest),
                UserControllerTests.TestDataEntryForCreate(
                    new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                // invalid password
                UserControllerTests.TestDataEntryForCreate(
                    password: new string(
                        'a',
                        7),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                UserControllerTests.TestDataEntryForCreate(
                    password: new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                // invalid roles
                UserControllerTests.TestDataEntryForCreate(
                    userRoles: Array.Empty<Role>(),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                UserControllerTests.TestDataEntryForCreate(
                    userRoles: Enumerable.Range(
                            0,
                            11)
                        .Select(_ => Role.Accessor)
                        .ToArray(),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                // invalid display name
                UserControllerTests.TestDataEntryForCreate(
                    displayName: "a",
                    expectedStatusCode: HttpStatusCode.BadRequest),
                UserControllerTests.TestDataEntryForCreate(
                    displayName: new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest)
            };

        /// <summary>
        ///     Gets the test data for the <see cref="Create" /> test.
        /// </summary>
        public static IEnumerable<object[]> TestDataForDelete =>
            new[]
            {
                // default test should pass
                UserControllerTests.TestDataEntryForDelete(),
                // api key errors
                UserControllerTests.TestDataEntryForDelete(
                    apiKey: "",
                    expectedStatusCode: HttpStatusCode.Unauthorized),
                UserControllerTests.TestDataEntryForDelete(
                    apiKey: Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // missing roles
                UserControllerTests.TestDataEntryForDelete(
                    roles: new Role[] { },
                    expectedStatusCode: HttpStatusCode.Forbidden),
                UserControllerTests.TestDataEntryForDelete(
                    roles: new[] {Role.User},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                UserControllerTests.TestDataEntryForDelete(
                    roles: new[] {Role.Admin},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                UserControllerTests.TestDataEntryForDelete(
                    roles: new[] {Role.Accessor},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // invalid id
                UserControllerTests.TestDataEntryForDelete(
                    getIdToDelete: _ => "a",
                    expectedStatusCode: HttpStatusCode.BadRequest),
                UserControllerTests.TestDataEntryForDelete(
                    getIdToDelete: _ => new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                // unknown id
                UserControllerTests.TestDataEntryForDelete(
                    getIdToDelete: _ => Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.NotFound)
            };

        /// <summary>
        ///     Gets the test data for the <see cref="ReadAll" /> test.
        /// </summary>
        public static IEnumerable<object[]> TestDataForReadAll =>
            new[]
            {
                // default test should pass
                UserControllerTests.TestDataEntryForReadAll(),
                // api key errors
                UserControllerTests.TestDataEntryForReadAll(
                    apiKey: "",
                    expectedStatusCode: HttpStatusCode.Unauthorized),
                UserControllerTests.TestDataEntryForReadAll(
                    apiKey: Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // missing roles
                UserControllerTests.TestDataEntryForReadAll(
                    roles: new Role[] { },
                    expectedStatusCode: HttpStatusCode.Forbidden),
                UserControllerTests.TestDataEntryForReadAll(
                    roles: new[] {Role.User},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                UserControllerTests.TestDataEntryForReadAll(
                    roles: new[] {Role.Admin},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                UserControllerTests.TestDataEntryForReadAll(
                    roles: new[] {Role.Accessor},
                    expectedStatusCode: HttpStatusCode.Forbidden)
            };

        /// <summary>
        ///     Gets the test data for the <see cref="ReadById" /> test.
        /// </summary>
        public static IEnumerable<object[]> TestDataForReadById =>
            new[]
            {
                // default test should pass
                UserControllerTests.TestDataEntryForReadById(),
                // api key errors
                UserControllerTests.TestDataEntryForReadById(
                    apiKey: "",
                    expectedStatusCode: HttpStatusCode.Unauthorized),
                UserControllerTests.TestDataEntryForReadById(
                    apiKey: Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // missing roles
                UserControllerTests.TestDataEntryForReadById(
                    roles: new Role[] { },
                    expectedStatusCode: HttpStatusCode.Forbidden),
                UserControllerTests.TestDataEntryForReadById(
                    roles: new[] {Role.User},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                UserControllerTests.TestDataEntryForReadById(
                    roles: new[] {Role.Admin},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                UserControllerTests.TestDataEntryForReadById(
                    roles: new[] {Role.Accessor},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // invalid id
                UserControllerTests.TestDataEntryForReadById(
                    getIdForRead: _ => "a",
                    expectedStatusCode: HttpStatusCode.BadRequest),
                UserControllerTests.TestDataEntryForReadById(
                    getIdForRead: _ => new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                // unknown id
                UserControllerTests.TestDataEntryForReadById(
                    getIdForRead: _ => Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.NotFound)
            };

        public static IEnumerable<object[]> TestDataForUpdate =>
            new[]
            {
                // default test should pass
                UserControllerTests.TestDataEntryForUpdate(),
                // api key errors
                UserControllerTests.TestDataEntryForUpdate(
                    apiKey: "",
                    expectedStatusCode: HttpStatusCode.Unauthorized),
                UserControllerTests.TestDataEntryForUpdate(
                    apiKey: Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // missing roles
                UserControllerTests.TestDataEntryForUpdate(
                    roles: new Role[] { },
                    expectedStatusCode: HttpStatusCode.Forbidden),
                UserControllerTests.TestDataEntryForUpdate(
                    roles: new[] {Role.User},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                UserControllerTests.TestDataEntryForUpdate(
                    roles: new[] {Role.Admin},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                UserControllerTests.TestDataEntryForUpdate(
                    roles: new[] {Role.Accessor},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // invalid id
                UserControllerTests.TestDataEntryForUpdate(
                    id2: "a",
                    expectedStatusCode: HttpStatusCode.BadRequest),
                UserControllerTests.TestDataEntryForUpdate(
                    id2: new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                UserControllerTests.TestDataEntryForUpdate(
                    id2: Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.NotFound),
                // invalid password
                UserControllerTests.TestDataEntryForUpdate(
                    password2: new string(
                        'a',
                        7),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                UserControllerTests.TestDataEntryForUpdate(
                    password2: new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                // invalid roles
                UserControllerTests.TestDataEntryForUpdate(
                    userRoles2: Array.Empty<Role>(),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                UserControllerTests.TestDataEntryForUpdate(
                    userRoles2: Enumerable.Range(
                            0,
                            11)
                        .Select(_ => Role.Accessor)
                        .ToArray(),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                // invalid display name
                UserControllerTests.TestDataEntryForUpdate(
                    displayName2: "a",
                    expectedStatusCode: HttpStatusCode.BadRequest),
                UserControllerTests.TestDataEntryForUpdate(
                    displayName2: new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest)
            };

        [Theory]
        [MemberData(nameof(UserControllerTests.TestDataForCreate))]
        public async Task Create(
            User createEntry,
            HttpStatusCode expectedStatusCode,
            IEnumerable<Role> roles,
            string apiKey
        )
        {
            await GenericCrudControllerTests.Create<User, User>(
                this.urnNamespace,
                createEntry,
                expectedStatusCode,
                (request, response) =>
                {
                    Assert.Equal(
                        request.Id,
                        response.Id);
                    Assert.Equal(
                        request.DisplayName,
                        response.DisplayName);
                    Assert.Equal(
                        request.Roles,
                        response.Roles);
                },
                roles,
                apiKey);
        }

        [Theory]
        [MemberData(nameof(UserControllerTests.TestDataForDelete))]
        public async Task Delete(
            User createEntry,
            HttpStatusCode expectedStatusCode,
            IEnumerable<Role> roles,
            string apiKey,
            Func<User, string> getIdToDelete
        )
        {
            await GenericCrudControllerTests.Delete<User, ResultUser>(
                this.urnNamespace,
                createEntry,
                expectedStatusCode,
                roles,
                getIdToDelete,
                apiKey);
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
                Urn.ReadAll
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

        [Theory]
        [MemberData(nameof(UserControllerTests.TestDataForReadAll))]
        public async Task ReadAll(
            IEnumerable<User> createEntries,
            HttpStatusCode expectedStatusCode,
            IEnumerable<Role> roles,
            string apiKey,
            Action<IEnumerable<ResultUser>, IEnumerable<ResultUser>> asserts
        )
        {
            await GenericCrudControllerTests.ReadAll(
                this.urnNamespace,
                createEntries,
                expectedStatusCode,
                asserts,
                roles,
                apiKey);
        }

        [Theory]
        [MemberData(nameof(UserControllerTests.TestDataForReadById))]
        public async Task ReadById(
            User createEntry,
            HttpStatusCode expectedStatusCode,
            Role[] roles,
            string apiKey,
            Func<ResultUser, string> getIdForRead
        )
        {
            await GenericCrudControllerTests.ReadById<User, ResultUser, ResultUser>(
                this.urnNamespace,
                createEntry,
                expectedStatusCode,
                (created, read) =>
                {
                    Assert.Equal(
                        created.Id,
                        read.Id);
                    Assert.Equal(
                        created.DisplayName,
                        read.DisplayName);
                    Assert.Equal(
                        created.Roles,
                        read.Roles);
                },
                roles,
                getIdForRead,
                apiKey);
        }

        public static object[] TestDataEntryForCreate(
            string? id = null,
            string? password = null,
            Role[]? userRoles = null,
            string? displayName = null,
            HttpStatusCode expectedStatusCode = HttpStatusCode.Created,
            string apiKey = TestFactory.ApiKey,
            Role[]? roles = null
        )
        {
            return new object[]
            {
                UserControllerTests.CreateUser(
                    id,
                    password,
                    userRoles,
                    displayName),
                expectedStatusCode,
                roles ??
                new[]
                {
                    Role.Accessor,
                    Role.Admin
                },
                apiKey
            };
        }

        public static object[] TestDataEntryForReadAll(
            string? id1 = null,
            string? password1 = null,
            Role[]? userRoles1 = null,
            string? displayName1 = null,
            string? id2 = null,
            string? password2 = null,
            Role[]? userRoles2 = null,
            string? displayName2 = null,
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK,
            string apiKey = TestFactory.ApiKey,
            Role[]? roles = null
        )
        {
            return new object[]
            {
                new[]
                {
                    UserControllerTests.CreateUser(
                        id1,
                        password1,
                        userRoles1,
                        displayName1),
                    UserControllerTests.CreateUser(
                        id2,
                        password2,
                        userRoles2,
                        displayName2)
                },
                expectedStatusCode,
                roles ??
                new[]
                {
                    Role.Accessor,
                    Role.Admin
                },
                apiKey,
                new Action<IEnumerable<ResultUser>, IEnumerable<ResultUser>>(
                    (createdTokenEntries, resultTokenEntries) =>
                    {
                        var createdResults = createdTokenEntries.ToArray();
                        var results = resultTokenEntries.ToArray();
                        Assert.True(createdResults.Length <= results.Length);
                        foreach (var createdResult in createdResults)
                        {
                            Assert.Contains(
                                results,
                                entry => entry.Id == createdResult.Id &&
                                         entry.DisplayName == createdResult.DisplayName &&
                                         entry.Roles.All(role => createdResult.Roles.Any(r => r == role)) &&
                                         entry.Roles.Count() == createdResult.Roles.Count());
                        }
                    })
            };
        }

        public static object[] TestDataEntryForReadById(
            string? id = null,
            string? password = null,
            Role[]? userRoles = null,
            string? displayName = null,
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK,
            string apiKey = TestFactory.ApiKey,
            Role[]? roles = null,
            Func<ResultUser, string>? getIdForRead = null
        )
        {
            return new object[]
            {
                UserControllerTests.CreateUser(
                    id,
                    password,
                    userRoles,
                    displayName),
                expectedStatusCode,
                roles ??
                new[]
                {
                    Role.Accessor,
                    Role.Admin
                },
                apiKey,
                getIdForRead ?? new Func<ResultUser, string>(entry => entry.Id)
            };
        }

        public static object[] TestDataEntryForUpdate(
            string? id1 = null,
            string? password1 = null,
            Role[]? userRoles1 = null,
            string? displayName1 = null,
            string? id2 = null,
            string? password2 = null,
            Role[]? userRoles2 = null,
            string? displayName2 = null,
            HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent,
            Role[]? roles = null,
            string apiKey = TestFactory.ApiKey
        )
        {
            var entry = UserControllerTests.CreateUser(
                id1,
                password1,
                userRoles1,
                displayName1);
            return new object[]
            {
                entry,
                UserControllerTests.CreateUser(
                    id2 ?? entry.Id,
                    password2,
                    userRoles2,
                    displayName2),
                expectedStatusCode,
                roles ??
                new[]
                {
                    Role.Admin,
                    Role.Accessor
                },
                apiKey
            };
        }

        [Theory]
        [MemberData(nameof(UserControllerTests.TestDataForUpdate))]
        public async Task Update(
            User createEntry,
            User updateEntry,
            HttpStatusCode expectedStatusCode,
            Role[] roles,
            string apiKey
        )
        {
            await GenericCrudControllerTests.Update<User, ResultUser, ResultUser, User>(
                this.urnNamespace,
                createEntry,
                updateEntry,
                expectedStatusCode,
                entry =>
                {
                    Assert.Equal(
                        updateEntry.Id,
                        entry.Id);
                    Assert.Equal(
                        updateEntry.DisplayName,
                        entry.DisplayName);
                    Assert.Equal(
                        updateEntry.Roles,
                        entry.Roles);
                },
                roles,
                apiKey);
        }

        private static User CreateUser(
            string? id = null,
            string? password = null,
            Role[]? roles = null,
            string? displayName = null
        )
        {
            return new User(
                id ?? Guid.NewGuid().ToString(),
                password ?? Guid.NewGuid().ToString(),
                roles ??
                new[]
                {
                    Role.User,
                    Role.Accessor
                },
                displayName ?? Guid.NewGuid().ToString());
        }

        private static object[] TestDataEntryForDelete(
            string? id = null,
            string? password = null,
            Role[]? userRoles = null,
            string? displayName = null,
            HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent,
            string apiKey = TestFactory.ApiKey,
            Role[]? roles = null,
            Func<User, string>? getIdToDelete = null
        )
        {
            return new object[]
            {
                UserControllerTests.CreateUser(
                    id,
                    password,
                    userRoles,
                    displayName),
                expectedStatusCode,
                roles ??
                new[]
                {
                    Role.Accessor,
                    Role.Admin
                },
                apiKey,
                getIdToDelete ?? new Func<User, string>(user => user.Id)
            };
        }
    }
}
