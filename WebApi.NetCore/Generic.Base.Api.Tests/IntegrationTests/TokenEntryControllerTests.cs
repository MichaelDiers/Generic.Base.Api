namespace Generic.Base.Api.Tests.IntegrationTests
{
    using System.Net;
    using Generic.Base.Api.AuthServices.TokenService;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory;

    /// <summary>
    ///     Tests for <see cref="TokenEntryController" />.
    /// </summary>
    public class TokenEntryControllerTests
    {
        /// <summary>
        ///     The default urn namespace for operations on <see cref="TokenEntry" />.
        /// </summary>
        private readonly string urnNamespace = nameof(TokenEntryController)[..^10];

        /// <summary>
        ///     Gets the test data for the <see cref="Create" /> test.
        /// </summary>
        public static IEnumerable<object[]> TestDataForCreate =>
            new[]
            {
                // default test should pass
                TokenEntryControllerTests.TestDataEntryForCreate(),
                // api key errors
                TokenEntryControllerTests.TestDataEntryForCreate(
                    apiKey: "",
                    expectedStatusCode: HttpStatusCode.Unauthorized),
                TokenEntryControllerTests.TestDataEntryForCreate(
                    apiKey: Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // missing roles
                TokenEntryControllerTests.TestDataEntryForCreate(
                    roles: new Role[] { },
                    expectedStatusCode: HttpStatusCode.Forbidden),
                TokenEntryControllerTests.TestDataEntryForCreate(
                    roles: new[] {Role.User},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                TokenEntryControllerTests.TestDataEntryForCreate(
                    roles: new[] {Role.Admin},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                TokenEntryControllerTests.TestDataEntryForCreate(
                    roles: new[] {Role.Accessor},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // invalid id
                TokenEntryControllerTests.TestDataEntryForCreate(
                    "a",
                    expectedStatusCode: HttpStatusCode.BadRequest),
                TokenEntryControllerTests.TestDataEntryForCreate(
                    new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                // invalid validUntil
                TokenEntryControllerTests.TestDataEntryForCreate(
                    validUntil: "invalid",
                    expectedStatusCode: HttpStatusCode.BadRequest),
                // invalid userId
                TokenEntryControllerTests.TestDataEntryForCreate(
                    userId: "a",
                    expectedStatusCode: HttpStatusCode.BadRequest),
                TokenEntryControllerTests.TestDataEntryForCreate(
                    userId: new string(
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
                TokenEntryControllerTests.TestDataEntryForDelete(),
                // api key errors
                TokenEntryControllerTests.TestDataEntryForDelete(
                    apiKey: "",
                    expectedStatusCode: HttpStatusCode.Unauthorized),
                TokenEntryControllerTests.TestDataEntryForDelete(
                    apiKey: Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // missing roles
                TokenEntryControllerTests.TestDataEntryForDelete(
                    roles: new Role[] { },
                    expectedStatusCode: HttpStatusCode.Forbidden),
                TokenEntryControllerTests.TestDataEntryForDelete(
                    roles: new[] {Role.User},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                TokenEntryControllerTests.TestDataEntryForDelete(
                    roles: new[] {Role.Admin},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                TokenEntryControllerTests.TestDataEntryForDelete(
                    roles: new[] {Role.Accessor},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // invalid id
                TokenEntryControllerTests.TestDataEntryForDelete(
                    getIdToDelete: _ => "a",
                    expectedStatusCode: HttpStatusCode.BadRequest),
                TokenEntryControllerTests.TestDataEntryForDelete(
                    getIdToDelete: _ => new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                // unknown id
                TokenEntryControllerTests.TestDataEntryForDelete(
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
                TokenEntryControllerTests.TestDataEntryForReadAll(),
                // api key errors
                TokenEntryControllerTests.TestDataEntryForReadAll(
                    apiKey: "",
                    expectedStatusCode: HttpStatusCode.Unauthorized),
                TokenEntryControllerTests.TestDataEntryForReadAll(
                    apiKey: Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // missing roles
                TokenEntryControllerTests.TestDataEntryForReadAll(
                    roles: new Role[] { },
                    expectedStatusCode: HttpStatusCode.Forbidden),
                TokenEntryControllerTests.TestDataEntryForReadAll(
                    roles: new[] {Role.User},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                TokenEntryControllerTests.TestDataEntryForReadAll(
                    roles: new[] {Role.Admin},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                TokenEntryControllerTests.TestDataEntryForReadAll(
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
                TokenEntryControllerTests.TestDataEntryForReadById(),
                // api key errors
                TokenEntryControllerTests.TestDataEntryForReadById(
                    apiKey: "",
                    expectedStatusCode: HttpStatusCode.Unauthorized),
                TokenEntryControllerTests.TestDataEntryForReadById(
                    apiKey: Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // missing roles
                TokenEntryControllerTests.TestDataEntryForReadById(
                    roles: new Role[] { },
                    expectedStatusCode: HttpStatusCode.Forbidden),
                TokenEntryControllerTests.TestDataEntryForReadById(
                    roles: new[] {Role.User},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                TokenEntryControllerTests.TestDataEntryForReadById(
                    roles: new[] {Role.Admin},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                TokenEntryControllerTests.TestDataEntryForReadById(
                    roles: new[] {Role.Accessor},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // invalid id
                TokenEntryControllerTests.TestDataEntryForReadById(
                    getIdForRead: _ => "a",
                    expectedStatusCode: HttpStatusCode.BadRequest),
                TokenEntryControllerTests.TestDataEntryForReadById(
                    getIdForRead: _ => new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                // unknown id
                TokenEntryControllerTests.TestDataEntryForReadById(
                    getIdForRead: _ => Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.NotFound)
            };

        public static IEnumerable<object[]> TestDataForUpdate =>
            new[]
            {
                // default test should pass
                TokenEntryControllerTests.TestDataEntryForUpdate(),
                // api key errors
                TokenEntryControllerTests.TestDataEntryForUpdate(
                    apiKey: "",
                    expectedStatusCode: HttpStatusCode.Unauthorized),
                TokenEntryControllerTests.TestDataEntryForUpdate(
                    apiKey: Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // missing roles
                TokenEntryControllerTests.TestDataEntryForUpdate(
                    roles: new Role[] { },
                    expectedStatusCode: HttpStatusCode.Forbidden),
                TokenEntryControllerTests.TestDataEntryForUpdate(
                    roles: new[] {Role.User},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                TokenEntryControllerTests.TestDataEntryForUpdate(
                    roles: new[] {Role.Admin},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                TokenEntryControllerTests.TestDataEntryForUpdate(
                    roles: new[] {Role.Accessor},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // invalid id
                TokenEntryControllerTests.TestDataEntryForUpdate(
                    id2: "a",
                    expectedStatusCode: HttpStatusCode.BadRequest),
                TokenEntryControllerTests.TestDataEntryForUpdate(
                    id2: new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                TokenEntryControllerTests.TestDataEntryForUpdate(
                    id2: Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.NotFound)
            };

        [Theory]
        [MemberData(nameof(TokenEntryControllerTests.TestDataForCreate))]
        public async Task Create(
            TokenEntry createEntry,
            HttpStatusCode expectedStatusCode,
            IEnumerable<Role> roles,
            string apiKey
        )
        {
            await GenericCrudControllerTests.Create<TokenEntry, TokenEntry>(
                this.urnNamespace,
                createEntry,
                expectedStatusCode,
                (request, response) =>
                {
                    Assert.Equal(
                        request.Id,
                        response.Id);
                    Assert.Equal(
                        request.UserId,
                        response.UserId);
                    Assert.Equal(
                        request.ValidUntil,
                        response.ValidUntil);
                },
                roles,
                apiKey);
        }

        [Theory]
        [MemberData(nameof(TokenEntryControllerTests.TestDataForDelete))]
        public async Task Delete(
            TokenEntry createEntry,
            HttpStatusCode expectedStatusCode,
            IEnumerable<Role> roles,
            string apiKey,
            Func<TokenEntry, string> getIdToDelete
        )
        {
            await GenericCrudControllerTests.Delete<TokenEntry, ResultTokenEntry>(
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
        [MemberData(nameof(TokenEntryControllerTests.TestDataForReadAll))]
        public async Task ReadAll(
            IEnumerable<TokenEntry> createEntries,
            HttpStatusCode expectedStatusCode,
            IEnumerable<Role> roles,
            string apiKey,
            Action<IEnumerable<ResultTokenEntry>, IEnumerable<ResultTokenEntry>> asserts
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
        [MemberData(nameof(TokenEntryControllerTests.TestDataForReadById))]
        public async Task ReadById(
            TokenEntry createEntry,
            HttpStatusCode expectedStatusCode,
            Role[] roles,
            string apiKey,
            Func<ResultTokenEntry, string> getIdForRead
        )
        {
            await GenericCrudControllerTests.ReadById<TokenEntry, ResultTokenEntry, ResultTokenEntry>(
                this.urnNamespace,
                createEntry,
                expectedStatusCode,
                (created, read) =>
                {
                    Assert.Equal(
                        created.Id,
                        read.Id);
                    Assert.Equal(
                        created.UserId,
                        read.UserId);
                    Assert.Equal(
                        created.ValidUntil,
                        read.ValidUntil);
                },
                roles,
                getIdForRead,
                apiKey);
        }

        public static object[] TestDataEntryForCreate(
            string? id = null,
            string? userId = null,
            string? validUntil = null,
            HttpStatusCode expectedStatusCode = HttpStatusCode.Created,
            string apiKey = TestFactory.ApiKey,
            Role[]? roles = null
        )
        {
            return new object[]
            {
                TokenEntryControllerTests.CreateTokenEntry(
                    id,
                    userId,
                    validUntil),
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
            string? userId1 = null,
            string? validUntil1 = null,
            string? id2 = null,
            string? userId2 = null,
            string? validUntil2 = null,
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK,
            string apiKey = TestFactory.ApiKey,
            Role[]? roles = null
        )
        {
            return new object[]
            {
                new[]
                {
                    TokenEntryControllerTests.CreateTokenEntry(
                        id1,
                        userId1,
                        validUntil1),
                    TokenEntryControllerTests.CreateTokenEntry(
                        id2,
                        userId2,
                        validUntil2)
                },
                expectedStatusCode,
                roles ??
                new[]
                {
                    Role.Accessor,
                    Role.Admin
                },
                apiKey,
                new Action<IEnumerable<ResultTokenEntry>, IEnumerable<ResultTokenEntry>>(
                    (createdTokenEntries, resultTokenEntries) =>
                    {
                        var createdResults = createdTokenEntries.ToArray();
                        var results = resultTokenEntries.ToArray();
                        Assert.Equal(
                            createdResults.Length,
                            results.Count());
                        foreach (var createdResult in createdResults)
                        {
                            Assert.Contains(
                                results,
                                entry => entry.Id == createdResult.Id &&
                                         entry.UserId == createdResult.UserId &&
                                         entry.ValidUntil == createdResult.ValidUntil);
                        }
                    })
            };
        }

        public static object[] TestDataEntryForReadById(
            string? id = null,
            string? userId = null,
            string? validUntil = null,
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK,
            string apiKey = TestFactory.ApiKey,
            Role[]? roles = null,
            Func<ResultTokenEntry, string>? getIdForRead = null
        )
        {
            return new object[]
            {
                TokenEntryControllerTests.CreateTokenEntry(
                    id,
                    userId,
                    validUntil),
                expectedStatusCode,
                roles ??
                new[]
                {
                    Role.Accessor,
                    Role.Admin
                },
                apiKey,
                getIdForRead ?? new Func<ResultTokenEntry, string>(entry => entry.Id)
            };
        }

        public static object[] TestDataEntryForUpdate(
            string? id1 = null,
            string? userId1 = null,
            string? validUntil1 = null,
            string? id2 = null,
            string? userId2 = null,
            string? validUntil2 = null,
            HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent,
            Role[]? roles = null,
            string apiKey = TestFactory.ApiKey
        )
        {
            var entry = TokenEntryControllerTests.CreateTokenEntry(
                id1,
                userId1,
                validUntil1);
            return new object[]
            {
                entry,
                TokenEntryControllerTests.CreateTokenEntry(
                    id2 ?? entry.Id,
                    userId2,
                    validUntil2),
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
        [MemberData(nameof(TokenEntryControllerTests.TestDataForUpdate))]
        public async Task Update(
            TokenEntry createEntry,
            TokenEntry updateEntry,
            HttpStatusCode expectedStatusCode,
            Role[] roles,
            string apiKey
        )
        {
            await GenericCrudControllerTests.Update<TokenEntry, ResultTokenEntry, ResultTokenEntry, TokenEntry>(
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
                        updateEntry.UserId,
                        entry.UserId);
                    Assert.Equal(
                        updateEntry.ValidUntil,
                        entry.ValidUntil);
                },
                roles,
                apiKey);
        }

        private static TokenEntry CreateTokenEntry(string? id = null, string? userId = null, string? validUntil = null)
        {
            return new TokenEntry(
                id ?? Guid.NewGuid().ToString(),
                userId ?? Guid.NewGuid().ToString(),
                validUntil ?? "2024.12.01 13:04:22");
        }

        private static object[] TestDataEntryForDelete(
            string? id = null,
            string? userId = null,
            string? validUntil = null,
            HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent,
            string apiKey = TestFactory.ApiKey,
            Role[]? roles = null,
            Func<TokenEntry, string>? getIdToDelete = null
        )
        {
            return new object[]
            {
                TokenEntryControllerTests.CreateTokenEntry(
                    id,
                    userId,
                    validUntil),
                expectedStatusCode,
                roles ??
                new[]
                {
                    Role.Accessor,
                    Role.Admin
                },
                apiKey,
                getIdToDelete ?? new Func<TokenEntry, string>(tokenEntry => tokenEntry.Id)
            };
        }
    }
}
