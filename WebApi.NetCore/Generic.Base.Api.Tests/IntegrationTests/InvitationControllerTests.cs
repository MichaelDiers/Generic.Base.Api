namespace Generic.Base.Api.Tests.IntegrationTests
{
    using System.Net;
    using Generic.Base.Api.AuthServices.InvitationService;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory;

    /// <summary>
    ///     Tests for <see cref="InvitationController" />.
    /// </summary>
    public class InvitationControllerTests
    {
        /// <summary>
        ///     The default urn namespace for operations on <see cref="Invitation" />.
        /// </summary>
        private readonly string urnNamespace = nameof(InvitationController)[..^10];

        /// <summary>
        ///     Gets the test data for the <see cref="Create" /> test.
        /// </summary>
        public static IEnumerable<object[]> TestDataForCreate =>
            new[]
            {
                // default test should pass
                InvitationControllerTests.TestDataEntryForCreate(),
                // api key errors
                InvitationControllerTests.TestDataEntryForCreate(
                    apiKey: "",
                    expectedStatusCode: HttpStatusCode.Unauthorized),
                InvitationControllerTests.TestDataEntryForCreate(
                    apiKey: Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // missing roles
                InvitationControllerTests.TestDataEntryForCreate(
                    roles: new Role[] { },
                    expectedStatusCode: HttpStatusCode.Forbidden),
                InvitationControllerTests.TestDataEntryForCreate(
                    roles: new[] {Role.User},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                InvitationControllerTests.TestDataEntryForCreate(
                    roles: new[] {Role.Admin},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                InvitationControllerTests.TestDataEntryForCreate(
                    roles: new[] {Role.Accessor},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // invalid id
                InvitationControllerTests.TestDataEntryForCreate(
                    "a",
                    expectedStatusCode: HttpStatusCode.BadRequest),
                InvitationControllerTests.TestDataEntryForCreate(
                    new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                // invalid roles
                InvitationControllerTests.TestDataEntryForCreate(
                    invitationRoles: Array.Empty<Role>(),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                InvitationControllerTests.TestDataEntryForCreate(
                    invitationRoles: Enumerable.Range(
                            0,
                            11)
                        .Select(_ => Role.Accessor)
                        .ToArray(),
                    expectedStatusCode: HttpStatusCode.BadRequest)
            };

        /// <summary>
        ///     Gets the test data for the <see cref="Create" /> test.
        /// </summary>
        public static IEnumerable<object[]> TestDataForDelete =>
            new[]
            {
                // default test should pass
                InvitationControllerTests.TestDataEntryForDelete(),
                // api key errors
                InvitationControllerTests.TestDataEntryForDelete(
                    apiKey: "",
                    expectedStatusCode: HttpStatusCode.Unauthorized),
                InvitationControllerTests.TestDataEntryForDelete(
                    apiKey: Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // missing roles
                InvitationControllerTests.TestDataEntryForDelete(
                    roles: new Role[] { },
                    expectedStatusCode: HttpStatusCode.Forbidden),
                InvitationControllerTests.TestDataEntryForDelete(
                    roles: new[] {Role.User},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                InvitationControllerTests.TestDataEntryForDelete(
                    roles: new[] {Role.Admin},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                InvitationControllerTests.TestDataEntryForDelete(
                    roles: new[] {Role.Accessor},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // invalid id
                InvitationControllerTests.TestDataEntryForDelete(
                    getIdToDelete: _ => "a",
                    expectedStatusCode: HttpStatusCode.BadRequest),
                InvitationControllerTests.TestDataEntryForDelete(
                    getIdToDelete: _ => new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                // unknown id
                InvitationControllerTests.TestDataEntryForDelete(
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
                InvitationControllerTests.TestDataEntryForReadAll(),
                // api key errors
                InvitationControllerTests.TestDataEntryForReadAll(
                    apiKey: "",
                    expectedStatusCode: HttpStatusCode.Unauthorized),
                InvitationControllerTests.TestDataEntryForReadAll(
                    apiKey: Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // missing roles
                InvitationControllerTests.TestDataEntryForReadAll(
                    roles: new Role[] { },
                    expectedStatusCode: HttpStatusCode.Forbidden),
                InvitationControllerTests.TestDataEntryForReadAll(
                    roles: new[] {Role.User},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                InvitationControllerTests.TestDataEntryForReadAll(
                    roles: new[] {Role.Admin},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                InvitationControllerTests.TestDataEntryForReadAll(
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
                InvitationControllerTests.TestDataEntryForReadById(),
                // api key errors
                InvitationControllerTests.TestDataEntryForReadById(
                    apiKey: "",
                    expectedStatusCode: HttpStatusCode.Unauthorized),
                InvitationControllerTests.TestDataEntryForReadById(
                    apiKey: Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // missing roles
                InvitationControllerTests.TestDataEntryForReadById(
                    roles: new Role[] { },
                    expectedStatusCode: HttpStatusCode.Forbidden),
                InvitationControllerTests.TestDataEntryForReadById(
                    roles: new[] {Role.User},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                InvitationControllerTests.TestDataEntryForReadById(
                    roles: new[] {Role.Admin},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                InvitationControllerTests.TestDataEntryForReadById(
                    roles: new[] {Role.Accessor},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // invalid id
                InvitationControllerTests.TestDataEntryForReadById(
                    getIdForRead: _ => "a",
                    expectedStatusCode: HttpStatusCode.BadRequest),
                InvitationControllerTests.TestDataEntryForReadById(
                    getIdForRead: _ => new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                // unknown id
                InvitationControllerTests.TestDataEntryForReadById(
                    getIdForRead: _ => Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.NotFound)
            };

        public static IEnumerable<object[]> TestDataForUpdate =>
            new[]
            {
                // default test should pass
                InvitationControllerTests.TestDataEntryForUpdate(),
                // api key errors
                InvitationControllerTests.TestDataEntryForUpdate(
                    apiKey: "",
                    expectedStatusCode: HttpStatusCode.Unauthorized),
                InvitationControllerTests.TestDataEntryForUpdate(
                    apiKey: Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // missing roles
                InvitationControllerTests.TestDataEntryForUpdate(
                    roles: new Role[] { },
                    expectedStatusCode: HttpStatusCode.Forbidden),
                InvitationControllerTests.TestDataEntryForUpdate(
                    roles: new[] {Role.User},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                InvitationControllerTests.TestDataEntryForUpdate(
                    roles: new[] {Role.Admin},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                InvitationControllerTests.TestDataEntryForUpdate(
                    roles: new[] {Role.Accessor},
                    expectedStatusCode: HttpStatusCode.Forbidden),
                // invalid id
                InvitationControllerTests.TestDataEntryForUpdate(
                    id2: "a",
                    expectedStatusCode: HttpStatusCode.BadRequest),
                InvitationControllerTests.TestDataEntryForUpdate(
                    id2: new string(
                        'a',
                        101),
                    expectedStatusCode: HttpStatusCode.BadRequest),
                InvitationControllerTests.TestDataEntryForUpdate(
                    id2: Guid.NewGuid().ToString(),
                    expectedStatusCode: HttpStatusCode.NotFound)
            };

        [Theory]
        [MemberData(nameof(InvitationControllerTests.TestDataForCreate))]
        public async Task Create(
            Invitation createEntry,
            HttpStatusCode expectedStatusCode,
            IEnumerable<Role> roles,
            string apiKey
        )
        {
            await GenericCrudControllerTests.Create<Invitation, Invitation>(
                this.urnNamespace,
                createEntry,
                expectedStatusCode,
                (request, response) =>
                {
                    Assert.Equal(
                        request.Id,
                        response.Id);
                    Assert.Equal(
                        request.Roles.Count(),
                        response.Roles.Count());
                    Assert.Equal(
                        request.Roles,
                        response.Roles);
                },
                roles,
                apiKey);
        }

        [Theory]
        [MemberData(nameof(InvitationControllerTests.TestDataForDelete))]
        public async Task Delete(
            Invitation createEntry,
            HttpStatusCode expectedStatusCode,
            IEnumerable<Role> roles,
            string apiKey,
            Func<Invitation, string> getIdToDelete
        )
        {
            await GenericCrudControllerTests.Delete<Invitation, ResultInvitation>(
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
        [MemberData(nameof(InvitationControllerTests.TestDataForReadAll))]
        public async Task ReadAll(
            IEnumerable<Invitation> createEntries,
            HttpStatusCode expectedStatusCode,
            IEnumerable<Role> roles,
            string apiKey,
            Action<IEnumerable<ResultInvitation>, IEnumerable<ResultInvitation>> asserts
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
        [MemberData(nameof(InvitationControllerTests.TestDataForReadById))]
        public async Task ReadById(
            Invitation createEntry,
            HttpStatusCode expectedStatusCode,
            Role[] roles,
            string apiKey,
            Func<ResultInvitation, string> getIdForRead
        )
        {
            await GenericCrudControllerTests.ReadById<Invitation, ResultInvitation, ResultInvitation>(
                this.urnNamespace,
                createEntry,
                expectedStatusCode,
                (created, read) =>
                {
                    Assert.Equal(
                        created.Id,
                        read.Id);
                    Assert.Equal(
                        created.Roles.Count(),
                        read.Roles.Count());
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
            Role[]? invitationRoles = null,
            HttpStatusCode expectedStatusCode = HttpStatusCode.Created,
            string apiKey = TestFactory.ApiKey,
            Role[]? roles = null
        )
        {
            return new object[]
            {
                InvitationControllerTests.CreateInvitation(
                    id,
                    invitationRoles),
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
            Role[]? invitationRoles1 = null,
            string? id2 = null,
            Role[]? invitationRoles2 = null,
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK,
            string apiKey = TestFactory.ApiKey,
            Role[]? roles = null
        )
        {
            return new object[]
            {
                new[]
                {
                    InvitationControllerTests.CreateInvitation(
                        id1,
                        invitationRoles1),
                    InvitationControllerTests.CreateInvitation(
                        id2,
                        invitationRoles2)
                },
                expectedStatusCode,
                roles ??
                new[]
                {
                    Role.Accessor,
                    Role.Admin
                },
                apiKey,
                new Action<IEnumerable<ResultInvitation>, IEnumerable<ResultInvitation>>(
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
                                         entry.Roles.Count() == createdResult.Roles.Count() &&
                                         entry.Roles.All(role => createdResult.Roles.Any(r => role == r)));
                        }
                    })
            };
        }

        public static object[] TestDataEntryForReadById(
            string? id = null,
            Role[]? invitationRoles = null,
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK,
            string apiKey = TestFactory.ApiKey,
            Role[]? roles = null,
            Func<ResultInvitation, string>? getIdForRead = null
        )
        {
            return new object[]
            {
                InvitationControllerTests.CreateInvitation(
                    id,
                    invitationRoles),
                expectedStatusCode,
                roles ??
                new[]
                {
                    Role.Accessor,
                    Role.Admin
                },
                apiKey,
                getIdForRead ?? new Func<ResultInvitation, string>(entry => entry.Id)
            };
        }

        public static object[] TestDataEntryForUpdate(
            string? id1 = null,
            Role[]? invitationRoles1 = null,
            string? id2 = null,
            Role[]? invitationRoles2 = null,
            HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent,
            Role[]? roles = null,
            string apiKey = TestFactory.ApiKey
        )
        {
            var entry = InvitationControllerTests.CreateInvitation(
                id1,
                invitationRoles1);
            return new object[]
            {
                entry,
                InvitationControllerTests.CreateInvitation(
                    id2 ?? entry.Id,
                    invitationRoles2),
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
        [MemberData(nameof(InvitationControllerTests.TestDataForUpdate))]
        public async Task Update(
            Invitation createEntry,
            Invitation updateEntry,
            HttpStatusCode expectedStatusCode,
            Role[] roles,
            string apiKey
        )
        {
            await GenericCrudControllerTests.Update<Invitation, ResultInvitation, ResultInvitation, Invitation>(
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
                        updateEntry.Roles.Count(),
                        entry.Roles.Count());
                    Assert.Equal(
                        updateEntry.Roles,
                        entry.Roles);
                },
                roles,
                apiKey);
        }

        private static Invitation CreateInvitation(string? id = null, Role[]? roles = null)
        {
            return new Invitation(
                id ?? Guid.NewGuid().ToString(),
                roles ??
                new[]
                {
                    Role.User,
                    Role.Accessor
                });
        }

        private static object[] TestDataEntryForDelete(
            string? id = null,
            Role[]? invitationRoles = null,
            HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent,
            string apiKey = TestFactory.ApiKey,
            Role[]? roles = null,
            Func<Invitation, string>? getIdToDelete = null
        )
        {
            return new object[]
            {
                InvitationControllerTests.CreateInvitation(
                    id,
                    invitationRoles),
                expectedStatusCode,
                roles ??
                new[]
                {
                    Role.Accessor,
                    Role.Admin
                },
                apiKey,
                getIdToDelete ?? new Func<Invitation, string>(invitation => invitation.Id)
            };
        }
    }
}
