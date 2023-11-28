namespace Generic.Base.Api.MongoDb.Tests.IntegrationTests
{
    using System.Net;
    using System.Net.Http.Json;
    using System.Text.RegularExpressions;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Database;
    using Generic.Base.Api.Models;
    using Generic.Base.Api.MongoDb.Tests.IntegrationTests.CustomWebApplicationFactory;

    /// <summary>
    ///     Generic tests for crud controllers.
    /// </summary>
    [Trait(
        "TestType",
        "MongoDbIntegrationTest")]
    public static class GenericCrudControllerTests
    {
        /// <summary>
        ///     Creates a new entry of type <typeparamref name="TCreate" />.
        /// </summary>
        /// <typeparam name="TCreate">The type of the data for creating a new entry.</typeparam>
        /// <typeparam name="TCreateResult">The type of the created entry.</typeparam>
        /// <param name="urnNamespace">The urn namespace for creating the entry.</param>
        /// <param name="createEntry">The data for creating a new entry.</param>
        /// <param name="statusCode">The expected status code.</param>
        /// <param name="createAsserts">The asserts for the created entry.</param>
        /// <param name="createRoles">The roles used for the create operation.</param>
        /// <param name="apiKey">The api used for the request.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the created entry.</returns>
        public static async Task<TCreateResult?> Create<TCreate, TCreateResult>(
            string urnNamespace,
            TCreate createEntry,
            HttpStatusCode statusCode,
            Action<TCreate, TCreateResult> createAsserts,
            IEnumerable<Role> createRoles,
            string? apiKey = TestFactory.ApiKey
        ) where TCreateResult : class
        {
            return await GenericCrudControllerTests.Create(
                TestFactory.GetClient(),
                urnNamespace,
                createEntry,
                statusCode,
                createAsserts,
                createRoles,
                apiKey);
        }

        /// <summary>
        ///     Generic test for a delete operation.
        /// </summary>
        /// <typeparam name="TCreate">The type of the data for creating an entry.</typeparam>
        /// <typeparam name="TCreateResult">The type of the created entry.</typeparam>
        /// <param name="urnNamespace">The urn namespace for the operation.</param>
        /// <param name="createEntry">The entry that is created.</param>
        /// <param name="statusCode">The expected status code.</param>
        /// <param name="deleteRoles">The roles that are used for the operation.</param>
        /// <param name="getIdToDelete">The get identifier to delete.</param>
        /// <param name="apiKey">The api key.</param>
        public static async Task Delete<TCreate, TCreateResult>(
            string urnNamespace,
            TCreate createEntry,
            HttpStatusCode statusCode,
            IEnumerable<Role> deleteRoles,
            Func<TCreate, string> getIdToDelete,
            string? apiKey = TestFactory.ApiKey
        ) where TCreateResult : class, ILinkResult where TCreate : IIdEntry
        {
            var client = TestFactory.GetClient();
            var created = await GenericCrudControllerTests.Create<TCreate, TCreateResult>(
                client,
                urnNamespace,
                createEntry,
                HttpStatusCode.Created,
                (_, _) => { },
                new[]
                {
                    Role.Admin,
                    Role.Accessor
                });

            Assert.NotNull(created);

            var deleteUrl = created.Links.FirstOrDefault(
                link => Regex.IsMatch(
                    link.Urn,
                    $"^urn:{urnNamespace}:{Urn.Delete.ToString()}"));

            Assert.NotNull(deleteUrl);

            await client.AddApiKey(apiKey)
                .AddToken(deleteRoles.ToArray())
                .DeleteAsync(
                    deleteUrl.Url.Replace(
                        createEntry.Id,
                        getIdToDelete(createEntry)),
                    statusCode);
        }

        /// <summary>
        ///     Generic test for a options operation.
        /// </summary>
        /// <param name="urnNamespace">The urn namespace of the operation.</param>
        /// <param name="statusCode">The expected status code.</param>
        /// <param name="roles">The roles used for the request.</param>
        /// <param name="urns">The expected urns.</param>
        /// <param name="apiKey">The api key.</param>
        public static async Task Options(
            string urnNamespace,
            HttpStatusCode statusCode,
            IEnumerable<Role> roles,
            IEnumerable<Urn> urns,
            string? apiKey = null
        )
        {
            await TestFactory.GetClient()
                .AddApiKey(apiKey)
                .AddToken(roles.ToArray())
                .OptionsAsync(
                    c => c.GetUrl(
                        urnNamespace,
                        Urn.Options),
                    statusCode,
                    urns,
                    urnNamespace);
        }

        /// <summary>
        ///     Generic test for a read all operation.
        /// </summary>
        /// <typeparam name="TCreate">The type of the create entry.</typeparam>
        /// <typeparam name="TCreateResult">The type of the create result.</typeparam>
        /// <typeparam name="TReadAllResult">The type of the read all result.</typeparam>
        /// <param name="urnNamespace">The urn namespace of the operation.</param>
        /// <param name="createEntries">The entries that are created.</param>
        /// <param name="statusCode">The expected status code.</param>
        /// <param name="asserts">The asserts executed if the operation succeeds.</param>
        /// <param name="readAllRoles">The roles used for executing the operation.</param>
        /// <param name="apiKey">The API key.</param>
        /// <returns>The result of the operation.</returns>
        public static async Task<IEnumerable<TReadAllResult>> ReadAll<TCreate, TCreateResult, TReadAllResult>(
            string urnNamespace,
            IEnumerable<TCreate> createEntries,
            HttpStatusCode statusCode,
            Action<IEnumerable<TCreateResult>, IEnumerable<TReadAllResult>> asserts,
            IEnumerable<Role> readAllRoles,
            string? apiKey
        ) where TCreateResult : class, ILinkResult
        {
            var client = TestFactory.GetClient();

            var createdResults = new List<TCreateResult>();
            foreach (var createEntry in createEntries)
            {
                var created = await GenericCrudControllerTests.Create<TCreate, TCreateResult>(
                    client,
                    urnNamespace,
                    createEntry,
                    HttpStatusCode.Created,
                    (_, _) => { },
                    new[]
                    {
                        Role.Admin,
                        Role.Accessor
                    });

                Assert.NotNull(created);
                createdResults.Add(created);
            }

            var readAllUrl = createdResults.First()
                .Links.FirstOrDefault(
                    link => Regex.IsMatch(
                        link.Urn,
                        $"^urn:{urnNamespace}:{Urn.ReadAll.ToString()}"))
                ?.Url;

            Assert.NotNull(readAllUrl);

            var readAllResult = await client.AddApiKey(apiKey)
                .AddToken(readAllRoles.ToArray())
                .GetAsync<IEnumerable<TReadAllResult>>(
                    readAllUrl,
                    statusCode,
                    _ => { });

            if ((int) statusCode < 200 || (int) statusCode > 299)
            {
                return Enumerable.Empty<TReadAllResult>();
            }

            Assert.NotNull(readAllResult);

            var results = readAllResult.ToArray();
            asserts(
                createdResults,
                results);

            return results;
        }

        /// <summary>
        ///     Generic test for a read by id operation.
        /// </summary>
        /// <typeparam name="TCreate">The type of the create entry.</typeparam>
        /// <typeparam name="TCreateResult">The type of the create result.</typeparam>
        /// <typeparam name="TReadByIdResult">The type of the read by identifier result.</typeparam>
        /// <param name="urnNamespace">The urn namespace.</param>
        /// <param name="createEntry">The create entry.</param>
        /// <param name="statusCode">The expected status code.</param>
        /// <param name="readByIdAsserts">The read by identifier asserts.</param>
        /// <param name="readByIdRoles">The read by identifier roles.</param>
        /// <param name="getIdForRead">The get identifier for read.</param>
        /// <param name="apiKey">The API key.</param>
        /// <returns>The result of the operation.</returns>
        public static async Task<TReadByIdResult?> ReadById<TCreate, TCreateResult, TReadByIdResult>(
            string urnNamespace,
            TCreate createEntry,
            HttpStatusCode statusCode,
            Action<TCreateResult, TReadByIdResult> readByIdAsserts,
            IEnumerable<Role> readByIdRoles,
            Func<TCreateResult, string> getIdForRead,
            string? apiKey = TestFactory.ApiKey
        ) where TCreateResult : class, ILinkResult, IIdEntry where TReadByIdResult : class
        {
            var client = TestFactory.GetClient();
            var created = await GenericCrudControllerTests.Create<TCreate, TCreateResult>(
                client,
                urnNamespace,
                createEntry,
                HttpStatusCode.Created,
                (_, _) => { },
                new[]
                {
                    Role.Admin,
                    Role.Accessor
                });

            Assert.NotNull(created);

            var readByIdUrl = created.Links.FirstOrDefault(
                link => Regex.IsMatch(
                    link.Urn,
                    $"^urn:{urnNamespace}:{Urn.ReadById.ToString()}"));

            Assert.NotNull(readByIdUrl);

            var readByIdResult = await client.AddApiKey(apiKey)
                .AddToken(readByIdRoles.ToArray())
                .GetAsync<TReadByIdResult>(
                    readByIdUrl.Url.Replace(
                        created.Id,
                        getIdForRead(created)),
                    statusCode,
                    _ => { });

            if ((int) statusCode < 200 || (int) statusCode > 299)
            {
                return null;
            }

            Assert.NotNull(readByIdResult);
            readByIdAsserts(
                created,
                readByIdResult);
            return readByIdResult;
        }

        /// <summary>
        ///     Generic test for a update operation.
        /// </summary>
        /// <typeparam name="TCreate">The type of the create.</typeparam>
        /// <typeparam name="TCreateResult">The type of the create result.</typeparam>
        /// <typeparam name="TReadByIdResult">The type of the read by identifier result.</typeparam>
        /// <typeparam name="TUpdate">The type of the update.</typeparam>
        /// <param name="urnNamespace">The urn namespace.</param>
        /// <param name="createEntry">The create entry.</param>
        /// <param name="updateEntry">The update entry.</param>
        /// <param name="statusCode">The expected status code.</param>
        /// <param name="updateAsserts">The update asserts.</param>
        /// <param name="updateRoles">The update roles.</param>
        /// <param name="apiKey">The API key.</param>
        /// <returns>The result of the operation.</returns>
        public static async Task<TReadByIdResult?> Update<TCreate, TCreateResult, TReadByIdResult, TUpdate>(
            string urnNamespace,
            TCreate createEntry,
            TUpdate updateEntry,
            HttpStatusCode statusCode,
            Action<TReadByIdResult> updateAsserts,
            IEnumerable<Role> updateRoles,
            string? apiKey = TestFactory.ApiKey
        ) where TCreateResult : class, ILinkResult, IIdEntry where TReadByIdResult : class where TUpdate : IIdEntry
        {
            var client = TestFactory.GetClient();
            var created = await GenericCrudControllerTests.Create<TCreate, TCreateResult>(
                client,
                urnNamespace,
                createEntry,
                HttpStatusCode.Created,
                (_, _) => { },
                new[]
                {
                    Role.Admin,
                    Role.Accessor
                });

            Assert.NotNull(created);

            var updateUrl = created.Links.FirstOrDefault(
                link => Regex.IsMatch(
                    link.Urn,
                    $"^urn:{urnNamespace}:{Urn.Update.ToString()}"));
            Assert.NotNull(updateUrl);

            await client.AddApiKey(apiKey)
            .AddToken(updateRoles.ToArray())
            .PutAsync(
                updateEntry,
                updateUrl.Url.Replace(
                    created.Id,
                    updateEntry.Id),
                statusCode);

            if ((int) statusCode < 200 || (int) statusCode > 299)
            {
                return null;
            }

            var readByIdUrl = created.Links.FirstOrDefault(
                link => Regex.IsMatch(
                    link.Urn,
                    $"^urn:{urnNamespace}:{Urn.ReadById.ToString()}"));

            Assert.NotNull(readByIdUrl);

            var readByIdResponse = await client.AddApiKey(apiKey)
                .AddToken(
                    Role.Admin,
                    Role.Accessor)
                .GetAsync(readByIdUrl.Url);

            Assert.Equal(
                HttpStatusCode.OK,
                readByIdResponse.StatusCode);

            if (!readByIdResponse.IsSuccessStatusCode)
            {
                return null;
            }

            var readByIdResult = await readByIdResponse.Content.ReadFromJsonAsync<TReadByIdResult>();
            Assert.NotNull(readByIdResult);
            updateAsserts(readByIdResult);
            return readByIdResult;
        }

        /// <summary>
        ///     Creates a new entry of type <typeparamref name="TCreate" />.
        /// </summary>
        /// <typeparam name="TCreate">The type of the data for creating a new entry.</typeparam>
        /// <typeparam name="TCreateResult">The type of the created entry.</typeparam>
        /// <param name="client">The http client.</param>
        /// <param name="urnNamespace">The urn namespace for creating the entry.</param>
        /// <param name="createEntry">The data for creating a new entry.</param>
        /// <param name="statusCode">The expected status code.</param>
        /// <param name="createAsserts">The asserts for the created entry.</param>
        /// <param name="createRoles">The roles used for the create operation.</param>
        /// <param name="apiKey">The api used for the request.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the created entry.</returns>
        private static async Task<TCreateResult?> Create<TCreate, TCreateResult>(
            HttpClient client,
            string urnNamespace,
            TCreate createEntry,
            HttpStatusCode statusCode,
            Action<TCreate, TCreateResult> createAsserts,
            IEnumerable<Role> createRoles,
            string? apiKey = TestFactory.ApiKey
        ) where TCreateResult : class
        {
            return await client.AddApiKey(apiKey)
            .AddToken(createRoles.ToArray())
            .PostAsync(
                createEntry,
                () => HttpClientExtensions.GetUrl(
                    urnNamespace,
                    Urn.Create),
                statusCode,
                createAsserts);
        }
    }
}
