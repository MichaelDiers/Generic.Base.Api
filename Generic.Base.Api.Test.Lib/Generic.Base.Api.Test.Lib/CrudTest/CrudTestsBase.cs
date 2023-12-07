namespace Generic.Base.Api.Test.Lib.CrudTest
{
    using System.Net;
    using System.Security.Claims;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Test.Lib.Extensions;
    using Microsoft.AspNetCore.Mvc.Testing;

    /// <summary>
    ///     Base class for user bound and user independent crud tests.
    /// </summary>
    /// <typeparam name="TEntryPoint">The type of the entry point.</typeparam>
    /// <typeparam name="TFactory">The type of the factory.</typeparam>
    /// <typeparam name="TCreate">The type of the data for creating a entry.</typeparam>
    /// <typeparam name="TCreateResult">The type of the create result.</typeparam>
    /// <typeparam name="TReadResult">The type of the read result.</typeparam>
    /// <typeparam name="TUpdate">The type of the data for updating an entry.</typeparam>
    /// <typeparam name="TUpdateResult">The type of the update result.</typeparam>
    public abstract class
        CrudTestsBase<TEntryPoint, TFactory, TCreate, TCreateResult, TReadResult, TUpdate, TUpdateResult>
        where TEntryPoint : class
        where TFactory : WebApplicationFactory<TEntryPoint>, new()
        where TCreate : class
        where TCreateResult : class, ILinkResult
        where TReadResult : class, ILinkResult
        where TUpdate : class
        where TUpdateResult : class
    {
        /// <summary>
        ///     A cache for requested urls.
        /// </summary>
        private readonly IDictionary<string, string> urlCache = new Dictionary<string, string>();

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="CrudTestsBase{TEntryPoint, TFactory, TCreate, TCreateResult, TReadResult, TUpdate, TUpdateResult}" />
        ///     class.
        /// </summary>
        /// <param name="apiKey">The valid API key.</param>
        protected CrudTestsBase(string apiKey)
        {
            this.ApiKey = apiKey;
            this.ClientHelper = new TFactory().CreateClient().AddApiKey(apiKey);
        }

        /// <summary>
        ///     Gets the expected api key.
        /// </summary>
        protected string ApiKey { get; }

        /// <summary>
        ///     Gets the client helper that is initialized with a valid api key.
        /// </summary>
        protected HttpClient ClientHelper { get; }

        /// <summary>
        ///     Gets test data for that the data validation fails in the create context.
        /// </summary>
        protected abstract IEnumerable<(TCreate createData, string testInfo)> CreateDataValidationFailsTestData { get; }

        /// <summary>
        ///     Gets the entry point URL that is supposed to be an options operation.
        /// </summary>
        protected abstract string EntryPointUrl { get; }

        /// <summary>
        ///     Gets an invalid id of an entry.
        /// </summary>
        protected abstract string GetInvalidId { get; }

        /// <summary>
        ///     Gets a valid id of an entry.
        /// </summary>
        protected abstract string GetValidId { get; }

        /// <summary>
        ///     Gets the roles that are required for options requests.
        /// </summary>
        protected abstract IEnumerable<Role> OptionsRoles { get; }

        /// <summary>
        ///     Gets a value indicating whether a double create raises a conflict.
        /// </summary>
        /// <value>
        ///     <c>true</c> if a double create raises a conflict; otherwise, <c>false</c>.
        /// </value>
        protected abstract bool RaiseDoubleCreateConflict { get; }

        /// <summary>
        ///     The roles that required for creating an entry.
        /// </summary>
        protected abstract IEnumerable<Role> RequiredCreateRoles { get; }

        /// <summary>
        ///     The roles that required for deleting an entry.
        /// </summary>
        protected abstract IEnumerable<Role> RequiredDeleteRoles { get; }

        /// <summary>
        ///     The roles that required for reading all entries.
        /// </summary>
        protected abstract IEnumerable<Role> RequiredReadAllRoles { get; }

        /// <summary>
        ///     The roles that required for reading an entry by its id.
        /// </summary>
        protected abstract IEnumerable<Role> RequiredReadByIdRoles { get; }

        /// <summary>
        ///     The roles that required for updating an entry.
        /// </summary>
        protected abstract IEnumerable<Role> RequiredUpdateRoles { get; }

        /// <summary>
        ///     Gets test data for that the data validation fails in the update context.
        /// </summary>
        protected abstract IEnumerable<(TUpdate updateData, string testInfo)> UpdateDataValidationFailsTestData { get; }

        /// <summary>
        ///     Gets the urn namespace for the services under test.
        /// </summary>
        protected abstract string UrnNamespace { get; }

        /// <summary>
        ///     The data validation fails for creating a new entry.
        /// </summary>
        [Fact]
        public async Task CreateDataValidationFails()
        {
            var client = new TFactory().CreateClient();
            var userId = Guid.NewGuid().ToString();

            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Create,
                client,
                userId,
                this.RequiredCreateRoles);

            client.Clear()
            .AddApiKey(this.ApiKey)
            .AddToken(
                this.GetClaims(
                    this.RequiredCreateRoles,
                    userId));
            foreach (var (create, info) in this.CreateDataValidationFailsTestData)
            {
                try
                {
                    await client.PostAsync<TCreate, TCreateResult>(
                        url,
                        create,
                        HttpStatusCode.BadRequest);
                }
                catch (Exception ex)
                {
                    Assert.Fail($"{info}: {ex}");
                }
            }
        }

        /// <summary>
        ///     Creating a new entry fails if a required role is missing.
        /// </summary>
        [Fact]
        public async Task CreateFailsIfRoleIsMissing()
        {
            await this.FailsIfRoleIsMissing(
                Urn.Create,
                this.RequiredCreateRoles,
                (client, url) => client.PostAsync<TCreate, TCreateResult>(
                    url,
                    this.GetValidCreateEntry(),
                    HttpStatusCode.Forbidden));
        }

        /// <summary>
        ///     Creating a new entry succeeds.
        /// </summary>
        [Fact]
        public async Task CreateSucceeds()
        {
            await this.Create();
        }

        /// <summary>
        ///     Creating a new entry fails using an invalid api key.
        /// </summary>
        [Fact]
        public async Task CreateUsingInvalidApiKeyFails()
        {
            var userId = Guid.NewGuid().ToString();
            var client = new TFactory().CreateClient();
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Create,
                client,
                userId,
                this.RequiredCreateRoles);

            await client.Clear()
                .AddApiKey(Guid.NewGuid().ToString())
                .AddToken(
                    this.GetClaims(
                        this.RequiredCreateRoles,
                        userId))
                .PostAsync<TCreate, TCreateResult>(
                    url,
                    this.GetValidCreateEntry(),
                    HttpStatusCode.Forbidden);
        }

        /// <summary>
        ///     Creating a new entry fails without an api key.
        /// </summary>
        [Fact]
        public async Task CreateWithoutApiKeyFails()
        {
            var userId = Guid.NewGuid().ToString();
            var client = new TFactory().CreateClient();
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Create,
                client,
                userId,
                this.RequiredCreateRoles);

            await client.Clear()
                .AddToken(
                    this.GetClaims(
                        this.RequiredCreateRoles,
                        userId))
                .PostAsync<TCreate, TCreateResult>(
                    url,
                    this.GetValidCreateEntry(),
                    HttpStatusCode.Unauthorized);
        }

        /// <summary>
        ///     Creating a new entry without a token fails.
        /// </summary>
        [Fact]
        public async Task CreateWithoutTokenFails()
        {
            var userId = Guid.NewGuid().ToString();
            var client = new TFactory().CreateClient();
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Create,
                client,
                userId,
                this.RequiredCreateRoles);

            await client.Clear()
            .AddApiKey(this.ApiKey)
            .PostAsync<TCreate, TCreateResult>(
                url,
                this.GetValidCreateEntry(),
                HttpStatusCode.Unauthorized);
        }

        /// <summary>
        ///     Deleting an entry fails if a role is missing.
        /// </summary>
        [Fact]
        public async Task DeleteFailsIfRoleIsMissing()
        {
            await this.FailsIfRoleIsMissing(
                Urn.Delete,
                this.RequiredDeleteRoles,
                (client, url) => client.DeleteAsync(
                    url,
                    HttpStatusCode.Forbidden));
        }

        /// <summary>
        ///     Deleting an entry succeeds.
        /// </summary>
        [Fact]
        public async Task DeleteSucceeds()
        {
            var client = new TFactory().CreateClient();
            var userId = Guid.NewGuid().ToString();
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Delete,
                client,
                userId,
                this.RequiredDeleteRoles);

            await client.Clear()
                .AddApiKey(this.ApiKey)
                .AddToken(
                    this.GetClaims(
                        this.RequiredDeleteRoles,
                        userId))
                .DeleteAsync(
                    url,
                    HttpStatusCode.NoContent);
        }

        /// <summary>
        ///     Deleting an entry fails using an invalid api key.
        /// </summary>
        [Fact]
        public async Task DeleteUsingInvalidApiKeyFails()
        {
            var userId = Guid.NewGuid().ToString();
            var client = new TFactory().CreateClient();
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Delete,
                client,
                userId,
                this.RequiredDeleteRoles);

            await client.Clear()
                .AddApiKey(Guid.NewGuid().ToString())
                .AddToken(
                    this.GetClaims(
                        this.RequiredDeleteRoles,
                        userId))
                .DeleteAsync(
                    url,
                    HttpStatusCode.Forbidden);
        }

        /// <summary>
        ///     Deleting an entry fails without an api key.
        /// </summary>
        [Fact]
        public async Task DeleteWithoutApiKeyFails()
        {
            var userId = Guid.NewGuid().ToString();
            var client = new TFactory().CreateClient();
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Delete,
                client,
                userId,
                this.RequiredDeleteRoles);

            await client.Clear()
                .AddToken(
                    this.GetClaims(
                        this.RequiredCreateRoles,
                        userId))
                .DeleteAsync(
                    url,
                    HttpStatusCode.Unauthorized);
        }

        /// <summary>
        ///     Deleting an entry without a token fails.
        /// </summary>
        [Fact]
        public async Task DeleteWithoutTokenFails()
        {
            var userId = Guid.NewGuid().ToString();
            var client = new TFactory().CreateClient();
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Delete,
                client,
                userId,
                this.RequiredDeleteRoles);

            await client.Clear()
            .AddApiKey(this.ApiKey)
            .DeleteAsync(
                url,
                HttpStatusCode.Unauthorized);
        }

        /// <summary>
        ///     Checks if repeated create raises a conflict.
        /// </summary>
        [Fact]
        public async Task DoubleCreateConflictCheck()
        {
            var userId = Guid.NewGuid().ToString();
            var client = new TFactory().CreateClient();
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Create,
                client,
                userId,
                this.RequiredCreateRoles);

            client.Clear()
            .AddApiKey(this.ApiKey)
            .AddToken(
                this.GetClaims(
                    this.RequiredCreateRoles,
                    userId));

            var create = this.GetValidCreateEntry();

            await client.PostAsync<TCreate, TCreateResult>(
                url,
                create,
                HttpStatusCode.Created);

            await client.PostAsync<TCreate, TCreateResult>(
                url,
                create,
                this.RaiseDoubleCreateConflict ? HttpStatusCode.Conflict : HttpStatusCode.Created);
        }

        /// <summary>
        ///     Checks if repeated create raises a conflict.
        /// </summary>
        [Fact]
        public async Task DoubleDeleteFails()
        {
            var userId = Guid.NewGuid().ToString();
            var client = new TFactory().CreateClient();
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Delete,
                client,
                userId,
                this.RequiredDeleteRoles);

            client.Clear()
            .AddApiKey(this.ApiKey)
            .AddToken(
                this.GetClaims(
                    this.RequiredDeleteRoles,
                    userId));

            await client.DeleteAsync(
                url,
                HttpStatusCode.NoContent);

            await client.DeleteAsync(
                url,
                HttpStatusCode.NotFound);
        }

        /// <summary>
        ///     Send an options request and check the result as anonymous.
        /// </summary>
        [Fact]
        public async Task OptionsAsAnonymous()
        {
            var client = new TFactory().CreateClient().AddApiKey(this.ApiKey);
            var linkResult = await client.OptionsAsync<LinkResult>(this.EntryPointUrl);

            var optionsUrl = linkResult.Links
                .FirstOrDefault(link => link.Urn == $"urn:{this.UrnNamespace}:{Urn.Options}")
                ?.Url;
            Assert.NotNull(optionsUrl);

            linkResult = await client.OptionsAsync<LinkResult>(optionsUrl);

            Assert.Contains(
                linkResult.Links,
                link => link.Urn == $"urn:{this.UrnNamespace}:{Urn.Options}");
            var count = 1;
            if (!this.RequiredCreateRoles.Any())
            {
                ++count;
                Assert.Contains(
                    linkResult.Links,
                    link => link.Urn == $"urn:{this.UrnNamespace}:{Urn.Create}");
            }

            if (!this.RequiredReadAllRoles.Any())
            {
                ++count;
                Assert.Contains(
                    linkResult.Links,
                    link => link.Urn == $"urn:{this.UrnNamespace}:{Urn.ReadAll}");
            }

            Assert.Equal(
                count,
                linkResult.Links.Count());
        }

        /// <summary>
        ///     Send an options request and check the result using create roles.
        /// </summary>
        [Fact]
        public async Task OptionsAsCreator()
        {
            var client = new TFactory().CreateClient().AddApiKey(this.ApiKey);
            var linkResult = await client.OptionsAsync<LinkResult>(this.EntryPointUrl);

            var optionsUrl = linkResult.Links
                .FirstOrDefault(link => link.Urn == $"urn:{this.UrnNamespace}:{Urn.Options}")
                ?.Url;
            Assert.NotNull(optionsUrl);

            linkResult = await client.AddToken(
                    this.GetClaims(
                        this.RequiredCreateRoles,
                        Guid.NewGuid().ToString()))
                .OptionsAsync<LinkResult>(optionsUrl);

            Assert.Contains(
                linkResult.Links,
                link => link.Urn == $"urn:{this.UrnNamespace}:{Urn.Options}");
            var count = 1;

            ++count;
            Assert.Contains(
                linkResult.Links,
                link => link.Urn == $"urn:{this.UrnNamespace}:{Urn.Create}");

            if (this.RequiredReadAllRoles.All(role1 => this.RequiredCreateRoles.Any(role2 => role1 == role2)))
            {
                ++count;
                Assert.Contains(
                    linkResult.Links,
                    link => link.Urn == $"urn:{this.UrnNamespace}:{Urn.ReadAll}");
            }

            Assert.Equal(
                count,
                linkResult.Links.Count());
        }

        /// <summary>
        ///     Send an options request and check the result using read all roles.
        /// </summary>
        [Fact]
        public async Task OptionsAsReadAll()
        {
            var client = new TFactory().CreateClient().AddApiKey(this.ApiKey);
            var linkResult = await client.OptionsAsync<LinkResult>(this.EntryPointUrl);

            var optionsUrl = linkResult.Links
                .FirstOrDefault(link => link.Urn == $"urn:{this.UrnNamespace}:{Urn.Options}")
                ?.Url;
            Assert.NotNull(optionsUrl);

            linkResult = await client.AddToken(
                    this.GetClaims(
                        this.RequiredReadAllRoles,
                        Guid.NewGuid().ToString()))
                .OptionsAsync<LinkResult>(optionsUrl);

            Assert.Contains(
                linkResult.Links,
                link => link.Urn == $"urn:{this.UrnNamespace}:{Urn.Options}");
            var count = 1;

            ++count;
            Assert.Contains(
                linkResult.Links,
                link => link.Urn == $"urn:{this.UrnNamespace}:{Urn.ReadAll}");

            if (this.RequiredCreateRoles.All(role1 => this.RequiredReadAllRoles.Any(role2 => role1 == role2)))
            {
                ++count;
                Assert.Contains(
                    linkResult.Links,
                    link => link.Urn == $"urn:{this.UrnNamespace}:{Urn.Create}");
            }

            Assert.Equal(
                count,
                linkResult.Links.Count());
        }

        /// <summary>
        ///     Reading all entries fails if a required role is missing.
        /// </summary>
        [Fact]
        public async Task ReadAllFailsIfRoleIsMissing()
        {
            await this.FailsIfRoleIsMissing(
                Urn.ReadAll,
                this.RequiredReadAllRoles,
                (client, url) => client.GetAsync<IEnumerable<TReadResult>>(
                    url,
                    HttpStatusCode.Forbidden));
        }

        /// <summary>
        ///     Reading all entries fails using an invalid api key.
        /// </summary>
        [Fact]
        public async Task ReadAllUsingInvalidApiKeyFails()
        {
            var userId = Guid.NewGuid().ToString();
            var client = new TFactory().CreateClient();
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.ReadAll,
                client,
                userId,
                this.RequiredReadAllRoles);

            await client.Clear()
                .AddApiKey(Guid.NewGuid().ToString())
                .AddToken(
                    this.GetClaims(
                        this.RequiredReadAllRoles,
                        userId))
                .GetAsync<IEnumerable<TReadResult>>(
                    url,
                    HttpStatusCode.Forbidden);
        }

        /// <summary>
        ///     Reading by id fails if the id is invalid.
        /// </summary>
        [Fact]
        public async Task ReadByIdFailsIfIdIsInvalid()
        {
            var userId = Guid.NewGuid().ToString();
            var client = new TFactory().CreateClient();
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.ReadById,
                client,
                userId,
                this.RequiredReadByIdRoles);

            url = string.Join(
                "/",
                url.Split("/")[..^1].Append(this.GetInvalidId));

            await client.Clear()
                .AddApiKey(this.ApiKey)
                .AddToken(
                    this.GetClaims(
                        this.RequiredReadByIdRoles,
                        userId))
                .GetAsync<TReadResult>(
                    url,
                    HttpStatusCode.BadRequest);
        }

        /// <summary>
        ///     Reading by id fails if the id is unknown.
        /// </summary>
        [Fact]
        public async Task ReadByIdFailsIfIdIsUnknown()
        {
            var userId = Guid.NewGuid().ToString();
            var client = new TFactory().CreateClient();
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.ReadById,
                client,
                userId,
                this.RequiredReadByIdRoles);

            url = string.Join(
                "/",
                url.Split("/")[..^1].Append(this.GetValidId));

            await client.Clear()
                .AddApiKey(this.ApiKey)
                .AddToken(
                    this.GetClaims(
                        this.RequiredReadByIdRoles,
                        userId))
                .GetAsync<TReadResult>(
                    url,
                    HttpStatusCode.NotFound);
        }

        /// <summary>
        ///     Reading by id fails if a role is missing.
        /// </summary>
        [Fact]
        public async Task ReadByIdFailsIfRoleIsMissing()
        {
            await this.FailsIfRoleIsMissing(
                Urn.ReadById,
                this.RequiredReadByIdRoles,
                (client, url) => client.GetAsync<TReadResult>(
                    url,
                    HttpStatusCode.Forbidden));
        }

        /// <summary>
        ///     Reading an entry by its id succeeds.
        /// </summary>
        [Fact]
        public async Task ReadByIdSucceeds()
        {
            var userId = Guid.NewGuid().ToString();
            var client = new TFactory().CreateClient();
            var createResult = await this.Create(
                client,
                userId);
            Assert.NotNull(createResult);
            var url = this.FindOperationUrl(
                createResult,
                Urn.ReadById);

            var readResult = await client.AddApiKey(this.ApiKey)
                .AddToken(
                    this.GetClaims(
                        this.RequiredReadByIdRoles,
                        userId))
                .GetAsync<TReadResult>(
                    url,
                    HttpStatusCode.OK);

            Assert.NotNull(readResult);
            this.AssertEntry(
                createResult,
                readResult);
        }

        /// <summary>
        ///     Reading an entry by id fails using an invalid api key.
        /// </summary>
        [Fact]
        public async Task ReadByIdUsingInvalidApiKeyFails()
        {
            var userId = Guid.NewGuid().ToString();
            var client = new TFactory().CreateClient();
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.ReadById,
                client,
                userId,
                this.RequiredReadByIdRoles);

            await client.Clear()
                .AddApiKey(Guid.NewGuid().ToString())
                .AddToken(
                    this.GetClaims(
                        this.RequiredReadByIdRoles,
                        userId))
                .GetAsync<TReadResult>(
                    url,
                    HttpStatusCode.Forbidden);
        }

        /// <summary>
        ///     The data validation fails for updating a new entry.
        /// </summary>
        [Fact]
        public async Task UpdateDataValidationFails()
        {
            var client = new TFactory().CreateClient();
            var userId = Guid.NewGuid().ToString();

            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Update,
                client,
                userId,
                this.RequiredUpdateRoles);

            client.Clear()
            .AddApiKey(this.ApiKey)
            .AddToken(
                this.GetClaims(
                    this.RequiredUpdateRoles,
                    userId));
            foreach (var (update, info) in this.UpdateDataValidationFailsTestData)
            {
                try
                {
                    await client.PutAsync<TUpdate, TUpdateResult>(
                        url,
                        update,
                        HttpStatusCode.BadRequest);
                }
                catch (Exception ex)
                {
                    Assert.Fail($"{info}: {ex}");
                }
            }
        }

        /// <summary>
        ///     Updating an entry fails if a required role is missing.
        /// </summary>
        [Fact]
        public async Task UpdateFailsIfRoleIsMissing()
        {
            await this.FailsIfRoleIsMissing(
                Urn.Update,
                this.RequiredUpdateRoles,
                (client, url) => client.PutAsync<TUpdate, TUpdateResult>(
                    url,
                    this.GetValidUpdateEntry(),
                    HttpStatusCode.Forbidden));
        }

        /// <summary>
        ///     Updating an entry succeeds.
        /// </summary>
        [Fact]
        public async Task UpdateSucceeds()
        {
            var userId = Guid.NewGuid().ToString();
            var client = new TFactory().CreateClient();
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Update,
                client,
                userId,
                this.RequiredUpdateRoles);

            await client.Clear()
                .AddApiKey(this.ApiKey)
                .AddToken(
                    this.GetClaims(
                        this.RequiredUpdateRoles,
                        userId))
                .PutAsync<TUpdate, TUpdateResult>(
                    url,
                    this.GetValidUpdateEntry(),
                    HttpStatusCode.NoContent);
        }

        /// <summary>
        ///     Updating an entry fails using an invalid api key.
        /// </summary>
        [Fact]
        public async Task UpdateUsingInvalidApiKeyFails()
        {
            var userId = Guid.NewGuid().ToString();
            var client = new TFactory().CreateClient();
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Update,
                client,
                userId,
                this.RequiredUpdateRoles);

            await client.Clear()
                .AddApiKey(Guid.NewGuid().ToString())
                .AddToken(
                    this.GetClaims(
                        this.RequiredUpdateRoles,
                        userId))
                .PutAsync<TUpdate, TUpdateResult>(
                    url,
                    this.GetValidUpdateEntry(),
                    HttpStatusCode.Forbidden);
        }

        /// <summary>
        ///     Asserts that the created entry matches the read result.
        /// </summary>
        /// <param name="createResult">The expected created result.</param>
        /// <param name="readResult">The actual read result that should match the created result.</param>
        protected abstract void AssertEntry(TCreateResult createResult, TReadResult readResult);

        /// <summary>
        ///     Create a new entry.
        /// </summary>
        /// <param name="httpClient">The optional http client.</param>
        /// <param name="userId">The optional id of the user.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the created entry.</returns>
        protected async Task<TCreateResult?> Create(HttpClient? httpClient = null, string? userId = null)
        {
            var client = httpClient ?? new TFactory().CreateClient();
            var validUserId = userId ?? Guid.NewGuid().ToString();
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Create,
                client,
                validUserId,
                this.RequiredCreateRoles);

            return await client.Clear()
                .AddApiKey(this.ApiKey)
                .AddToken(
                    this.GetClaims(
                        this.RequiredCreateRoles,
                        validUserId))
                .PostAsync<TCreate, TCreateResult>(
                    url,
                    this.GetValidCreateEntry(),
                    HttpStatusCode.Created);
        }

        /// <summary>
        ///     Find the url depending on the given urn.
        /// </summary>
        /// <param name="linkResult">The link result to evaluate.</param>
        /// <param name="urn">The requested operation.</param>
        /// <returns>The requested url.</returns>
        protected string FindOperationUrl(ILinkResult linkResult, Urn urn)
        {
            return this.FindOperationUrl(
                linkResult,
                this.UrnNamespace,
                urn);
        }

        /// <summary>
        ///     Find the url depending on the given urn.
        /// </summary>
        /// <param name="linkResult">The link result to evaluate.</param>
        /// <param name="urnNamespace">The urn namespace.</param>
        /// <param name="urn">The requested operation.</param>
        /// <returns>The requested url.</returns>
        protected string FindOperationUrl(ILinkResult linkResult, string urnNamespace, Urn urn)
        {
            var url = linkResult.Links.FirstOrDefault(link => link.Urn == $"urn:{urnNamespace}:{Urn.ReadById}")?.Url;
            if (string.IsNullOrWhiteSpace(url))
            {
                Assert.Fail(
                    $"Cannot find {urn} link using {urnNamespace} in: {string.Join(";", linkResult.Links.Select(link => link.Urn))}");
            }

            return url;
        }

        /// <summary>
        ///     Get the claims depending on given roles and user id.
        /// </summary>
        /// <param name="roles">The roles of the user.</param>
        /// <param name="userId">The id of the user.</param>
        /// <returns>The requested claims.</returns>
        protected abstract IEnumerable<Claim> GetClaims(IEnumerable<Role> roles, string userId);

        /// <summary>
        ///     Gets the url depending on urn and its namespace.
        /// </summary>
        /// <param name="urnNamespace">The requested urn namespace.</param>
        /// <param name="urn">The requested operation.</param>
        /// <param name="httpClient">The http client.</param>
        /// <param name="userId">The id of the user.</param>
        /// <param name="requiredRoles">The required roles for executing the operation.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the requested url.</returns>
        protected async Task<string> GetUrl(
            string urnNamespace,
            Urn urn,
            HttpClient httpClient,
            string userId,
            IEnumerable<Role> requiredRoles
        )
        {
            var key = $"urn:{urnNamespace}:{urn}";
            if (this.urlCache.TryGetValue(
                    key,
                    out var url))
            {
                return url;
            }

            var client = httpClient;

            var optionsResultLink = await this.GetUrl(
                this.UrnNamespace,
                Urn.Options,
                client,
                this.EntryPointUrl,
                true,
                userId,
                this.OptionsRoles);
            Assert.NotNull(optionsResultLink);
            if (optionsResultLink.Urn == key)
            {
                return optionsResultLink.Url;
            }

            optionsResultLink = await this.GetUrl(
                urnNamespace,
                urn,
                client,
                optionsResultLink.Url,
                false,
                userId,
                requiredRoles.Concat(this.RequiredCreateRoles));

            if (optionsResultLink is not null && key == optionsResultLink.Urn)
            {
                return optionsResultLink.Url;
            }

            if (!this.urlCache.TryGetValue(
                    $"urn:{urnNamespace}:{Urn.Create}",
                    out var createUrl))
            {
                Assert.Fail($"Cannot find urn:{urnNamespace}:{Urn.Create}");
            }

            var createResult = await client.AddToken(
                    this.GetClaims(
                        this.RequiredCreateRoles,
                        userId))
                .PostAsync<TCreate, TCreateResult>(
                    createUrl,
                    this.GetValidCreateEntry(),
                    HttpStatusCode.Created);
            Assert.NotNull(createResult);

            var requestedUrl = createResult.Links.FirstOrDefault(link => link.Urn == key)?.Url;

            if (string.IsNullOrWhiteSpace(requestedUrl))
            {
                Assert.Fail($"Cannot find url for {key}");
            }

            return requestedUrl;
        }

        /// <summary>
        ///     Gets a valid create entry.
        /// </summary>
        protected abstract TCreate GetValidCreateEntry();

        /// <summary>
        ///     Gets a valid update entry.
        /// </summary>
        protected abstract TUpdate GetValidUpdateEntry();

        /// <summary>
        ///     Execute a test that checks if an operation fails without a required role.
        /// </summary>
        /// <param name="urn">The operation to be executed.</param>
        /// <param name="requiredRoles">All required roles for executing the operation.</param>
        /// <param name="execute">A function that executes the operation.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        private async Task FailsIfRoleIsMissing(
            Urn urn,
            IEnumerable<Role> requiredRoles,
            Func<HttpClient, string, Task> execute
        )
        {
            var userId = Guid.NewGuid().ToString();
            var client = new TFactory().CreateClient();
            var allRequiredRoles = requiredRoles.ToArray();

            var url = await this.GetUrl(
                this.UrnNamespace,
                urn,
                client,
                userId,
                allRequiredRoles);

            client.Clear().AddApiKey(this.ApiKey);

            // iterate roles
            foreach (var requiredRole in allRequiredRoles)
            {
                var roles = new List<Role>(allRequiredRoles);
                roles.Remove(requiredRole);

                await execute(
                    client.AddToken(
                        this.GetClaims(
                            roles,
                            userId)),
                    url);
            }
        }

        /// <summary>
        ///     Gets the link depending on urn and its namespace.
        /// </summary>
        /// <param name="urnNamespace">The requested urn namespace.</param>
        /// <param name="urn">The requested operation.</param>
        /// <param name="client">The http client.</param>
        /// <param name="url">The options url to be used.</param>
        /// <param name="checkResult">Check the result of the given <paramref name="url" />.</param>
        /// <param name="userId">The id of the user.</param>
        /// <param name="requiredRoles">The required roles for executing the operation.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the requested link.</returns>
        private async Task<Link?> GetUrl(
            string urnNamespace,
            Urn urn,
            HttpClient client,
            string url,
            bool checkResult,
            string userId,
            IEnumerable<Role> requiredRoles
        )
        {
            var optionsResult = await client.AddApiKey(this.ApiKey)
                .AddToken(
                    this.GetClaims(
                        requiredRoles,
                        userId))
                .OptionsAsync<LinkResult>(url);
            optionsResult.Links.Where(link => !this.urlCache.ContainsKey(link.Urn))
                .ToList()
                .ForEach(
                    link =>
                    {
                        this.urlCache.Add(
                            link.Urn,
                            link.Url);
                    });

            var link = optionsResult.Links.FirstOrDefault(
                optionsLink => optionsLink.Urn == $"urn:{urnNamespace}:{urn}");
            if (checkResult && link is null)
            {
                Assert.Fail(
                    $"Cannot find urn:{urnNamespace}:{urn} in {string.Join(",", optionsResult.Links.Select(l => l.Urn))}");
            }

            return link;
        }
    }
}
