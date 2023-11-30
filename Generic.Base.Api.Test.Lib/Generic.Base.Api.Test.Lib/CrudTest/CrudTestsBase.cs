namespace Generic.Base.Api.Test.Lib.CrudTest
{
    using System.Net;
    using System.Security.Claims;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Exceptions;
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Test.Lib.Extensions;
    using Microsoft.AspNetCore.Mvc.Testing;

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
        private readonly IDictionary<string, string> urlCache = new Dictionary<string, string>();

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

        protected abstract IEnumerable<Role> RequiredCreateRoles { get; }
        protected abstract IEnumerable<Role> RequiredDeleteRoles { get; }
        protected abstract IEnumerable<Role> RequiredReadAllRoles { get; }
        protected abstract IEnumerable<Role> RequiredReadByIdRoles { get; }
        protected abstract IEnumerable<Role> RequiredUpdateRoles { get; }
        protected abstract string UrnNamespace { get; }

        [Fact]
        public async Task CreateDataValidationFails()
        {
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Create);
            var client = new TFactory().CreateClient()
                .AddApiKey(this.ApiKey)
                .AddToken(
                    this.GetClaims(
                        this.RequiredCreateRoles,
                        Guid.NewGuid().ToString()));
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

        [Fact]
        public async Task CreateFailsIfRoleIsMissing()
        {
            await this.FailsIfRoleIsMissing(
                Urn.Create,
                this.RequiredCreateRoles,
                (client, url) => client.PostAsync<TCreate, TCreateResult>(
                    url,
                    null,
                    HttpStatusCode.Forbidden));
        }

        /*
        [Fact]
        public async Task CreateFailsWithoutUserIdClaim()
        {
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Create);
            await new TFactory().CreateClient()
                .AddApiKey(this.ApiKey)
                .AddToken(this.RequiredCreateRoles)
                .PostAsync<TCreate, TCreateResult>(
                    url,
                    this.GetValidCreateEntry(),
                    HttpStatusCode.Unauthorized);
        }
        */
        [Fact]
        public async Task CreateSucceeds()
        {
            await this.Create();
        }

        [Fact]
        public async Task CreateUsingInvalidApiKeyFails()
        {
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Create);

            await new TFactory().CreateClient()
                .AddApiKey(Guid.NewGuid().ToString())
                .PostAsync<TCreate, TCreateResult>(
                    url,
                    null,
                    HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task CreateWithoutApiKeyFails()
        {
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Create);

            await new TFactory().CreateClient()
            .PostAsync<TCreate, TCreateResult>(
                url,
                null,
                HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CreateWithoutTokenFails()
        {
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Create);

            await new TFactory().CreateClient()
            .AddApiKey(this.ApiKey)
            .PostAsync<TCreate, TCreateResult>(
                url,
                null,
                HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task DoubleCreateConflictCheck()
        {
            var userId = Guid.NewGuid().ToString();
            var claims = this.GetClaims(
                    this.RequiredCreateRoles,
                    userId)
                .ToArray();

            var create = this.GetValidCreateEntry();
            var client = new TFactory().CreateClient();
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Create);
            await client.AddApiKey(this.ApiKey)
            .AddToken(claims)
            .PostAsync<TCreate, TCreateResult>(
                url,
                create,
                HttpStatusCode.Created);

            await client.AddApiKey(this.ApiKey)
            .AddToken(claims)
            .PostAsync<TCreate, TCreateResult>(
                url,
                create,
                this.RaiseDoubleCreateConflict ? HttpStatusCode.Conflict : HttpStatusCode.Created);
        }

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
            foreach (var createResultLink in createResult.Links)
            {
                Assert.Contains(
                    readResult.Links,
                    link => link.Url == createResultLink.Url && link.Urn == createResultLink.Urn);
            }
        }

        /// <summary>
        ///     Asserts that the created entry matches the read result.
        /// </summary>
        /// <param name="createResult">The expected created result.</param>
        /// <param name="readResult">The actual read result that should match the created result.</param>
        protected abstract void AssertEntry(TCreateResult createResult, TReadResult readResult);

        protected async Task<TCreateResult?> Create(HttpClient? httpClient = null, string? userId = null)
        {
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Create);
            var client = httpClient ?? new TFactory().CreateClient();
            return await client.AddApiKey(this.ApiKey)
                .AddToken(
                    this.GetClaims(
                        this.RequiredCreateRoles,
                        userId ?? Guid.NewGuid().ToString()))
                .PostAsync<TCreate, TCreateResult>(
                    url,
                    this.GetValidCreateEntry(),
                    HttpStatusCode.Created);
        }

        protected string FindOperationUrl(ILinkResult linkResult, Urn urn)
        {
            return this.FindOperationUrl(
                linkResult,
                this.UrnNamespace,
                urn);
        }

        protected string FindOperationUrl(ILinkResult linkResult, string urnNamespace, Urn urn)
        {
            var url = linkResult.Links.FirstOrDefault(link => link.Urn == $"urn:${urnNamespace}:{Urn.ReadById}")?.Url;
            if (string.IsNullOrWhiteSpace(url))
            {
                Assert.Fail(
                    $"Cannot find {urn} link using {urnNamespace} in: {string.Join(";", linkResult.Links.Select(link => link.Urn))}");
            }

            return url;
        }

        protected abstract IEnumerable<Claim> GetClaims(IEnumerable<Role> roles, string userId);

        protected async Task<string> GetUrl(string urnNamespace, Urn urn)
        {
            var key = $"urn:{urnNamespace}:{urn}";

            if (this.urlCache.TryGetValue(
                    key,
                    out var url))
            {
                return url;
            }

            var links = new Queue<string>();
            var visited = new List<string>();

            links.Enqueue(this.EntryPointUrl);
            while (links.Any())
            {
                var next = links.Dequeue();
                visited.Add(next);
                var linkResult = await this.ClientHelper.AddApiKey(this.ApiKey)
                    .AddToken(this.OptionsRoles)
                    .OptionsAsync<LinkResult>(next);
                foreach (var linkResultLink in linkResult.Links)
                {
                    if (linkResultLink.Urn.Split(":")[2] != Urn.Options.ToString())
                    {
                        this.urlCache[linkResultLink.Urn] = linkResultLink.Url;
                    }
                    else if (!visited.Contains(linkResultLink.Url))
                    {
                        links.Enqueue(linkResultLink.Url);
                    }
                }
            }

            if (this.urlCache.TryGetValue(
                    key,
                    out url))
            {
                return url;
            }

            throw new NotFoundException($"No url found for {urnNamespace} and {urn} in options request.");
        }
        /*
        [Fact]
        public async Task DoubleCreateCausesNoConflict()
        {
            var create = this.GetValidCreateEntry();
            var client = new TFactory().CreateClient();
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Create);
            await client.AddApiKey(this.ApiKey)
                .AddToken(
                    this.RequiredCreateRoles,
                    Guid.NewGuid().ToString())
                .PostAsync<TCreate, TCreateResult>(
                    url,
                    create,
                    HttpStatusCode.Created);
            await client.AddApiKey(this.ApiKey)
                .AddToken(
                    this.RequiredCreateRoles,
                    Guid.NewGuid().ToString())
                .PostAsync<TCreate, TCreateResult>(
                    url,
                    create,
                    HttpStatusCode.Created);
        }
        */
        /*
        [Fact]
        public async Task DeleteFailsIfRoleIsMissing()
        {
            var (httpClient, url) = await this.GetValidDeleteUrl();
            await this.FailsIfRoleIsMissing(
                this.RequiredDeleteRoles,
                client => client.DeleteAsync(
                    url,
                    HttpStatusCode.Forbidden),
                httpClient);
        }

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

        [Fact]
        public async Task UpdateFailsIfRoleIsMissing()
        {
            await this.FailsIfRoleIsMissing(
                Urn.Update,
                this.RequiredUpdateRoles,
                (client, url) => client.PutAsync<TUpdate, TUpdateResult>(
                    url,
                    null,
                    HttpStatusCode.Forbidden));
        }*/

        protected abstract TCreate GetValidCreateEntry();

        protected async Task<(HttpClient, string)> GetValidDeleteUrl(HttpClient? httpClient = null)
        {
            var client = httpClient ?? new TFactory().CreateClient();
            var (_, created) = await this.CallCreateAsync(client);
            var url = created.Links.First(link => link.Urn == $"urn:{this.UrnNamespace}:{Urn.Delete}").Url;
            return (client, url);
        }

        protected abstract TUpdate GetValidUpdateEntry();

        private async Task<(HttpClient client, TCreateResult createResult)> CallCreateAsync(
            HttpClient? httpClient = null
        )
        {
            var client = httpClient?.Clear() ?? new TFactory().CreateClient();

            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Create);

            var created = await client.AddApiKey(this.ApiKey)
                .AddToken(this.RequiredCreateRoles)
                .PostAsync<TCreate, TCreateResult>(
                    url,
                    this.GetValidCreateEntry(),
                    HttpStatusCode.Created);

            Assert.NotNull(created);
            client.Clear();

            return (client, created);
        }

        private async Task FailsIfRoleIsMissing(
            IEnumerable<Role> requiredRoles,
            Func<HttpClient, Task> execute,
            HttpClient? httpClient = null
        )
        {
            var client = httpClient?.Clear() ?? new TFactory().CreateClient();

            var allRequiredRoles = requiredRoles.ToArray();
            foreach (var requiredRole in allRequiredRoles)
            {
                var roles = new List<Role>(allRequiredRoles);
                roles.Remove(requiredRole);

                await execute(new TFactory().CreateClient().AddApiKey(this.ApiKey).AddToken(roles));
            }
        }

        private async Task FailsIfRoleIsMissing(
            Urn urn,
            IEnumerable<Role> requiredRoles,
            Func<HttpClient, string, Task> execute
        )
        {
            var url = await this.GetUrl(
                this.UrnNamespace,
                urn);

            var allRequiredRoles = requiredRoles.ToArray();
            foreach (var requiredRole in allRequiredRoles)
            {
                var roles = new List<Role>(allRequiredRoles);
                roles.Remove(requiredRole);

                await execute(
                    new TFactory().CreateClient().AddApiKey(this.ApiKey).AddToken(roles),
                    url);
            }
        }
    }
}
