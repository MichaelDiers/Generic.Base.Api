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
        where TReadResult : class
        where TUpdate : class
        where TUpdateResult : class

    {
        private readonly IDictionary<string, string> urlCache = new Dictionary<string, string>();

        protected CrudTestsBase(
            string urnNamespace,
            string entryPointUrl,
            string apiKey,
            IEnumerable<Role> optionsRoles,
            IEnumerable<Role> requiredCreateRoles,
            IEnumerable<Role> requiredReadAllRoles,
            IEnumerable<Role> requiredReadByIdRoles,
            IEnumerable<Role> requiredUpdateRoles,
            IEnumerable<Role> requiredDeleteRoles
        )
        {
            this.RequiredCreateRoles = requiredCreateRoles;
            this.RequiredReadAllRoles = requiredReadAllRoles;
            this.RequiredReadByIdRoles = requiredReadByIdRoles;
            this.RequiredUpdateRoles = requiredUpdateRoles;
            this.RequiredDeleteRoles = requiredDeleteRoles;
            this.UrnNamespace = urnNamespace;
            this.EntryPointUrl = entryPointUrl;
            this.ApiKey = apiKey;
            this.OptionsRoles = optionsRoles;
            this.ClientHelper = new TFactory().CreateClient().AddApiKey(apiKey);
        }

        protected string ApiKey { get; }
        protected HttpClient ClientHelper { get; }
        protected string EntryPointUrl { get; }
        protected IEnumerable<Role> OptionsRoles { get; }

        protected abstract bool RaiseDoubleCreateConflict { get; }
        protected IEnumerable<Role> RequiredCreateRoles { get; }
        protected IEnumerable<Role> RequiredDeleteRoles { get; }
        protected IEnumerable<Role> RequiredReadAllRoles { get; }
        protected IEnumerable<Role> RequiredReadByIdRoles { get; }
        protected IEnumerable<Role> RequiredUpdateRoles { get; }
        protected string UrnNamespace { get; }

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
            var url = await this.GetUrl(
                this.UrnNamespace,
                Urn.Create);
            await new TFactory().CreateClient()
                .AddApiKey(this.ApiKey)
                .AddToken(
                    this.GetClaims(
                        this.RequiredCreateRoles,
                        Guid.NewGuid().ToString()))
                .PostAsync<TCreate, TCreateResult>(
                    url,
                    this.GetValidCreateEntry(),
                    HttpStatusCode.Created);
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
