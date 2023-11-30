namespace Generic.Base.Api.Test.Lib.CrudTest
{
    using System.Net;
    using System.Security.Claims;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Test.Lib.Extensions;
    using Microsoft.AspNetCore.Mvc.Testing;

    public abstract class
        UserBoundCrudTests<TEntryPoint, TFactory, TCreate, TCreateResult, TReadResult, TUpdate, TUpdateResult>
        : CrudTestsBase<TEntryPoint, TFactory, TCreate, TCreateResult, TReadResult, TUpdate, TUpdateResult>
        where TEntryPoint : class
        where TFactory : WebApplicationFactory<TEntryPoint>, new()
        where TCreate : class
        where TCreateResult : class, ILinkResult
        where TReadResult : class, ILinkResult
        where TUpdate : class
        where TUpdateResult : class

    {
        protected UserBoundCrudTests(string apiKey)
            : base(apiKey)
        {
        }

        protected override bool RaiseDoubleCreateConflict => false;

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

        [Fact]
        public async Task ReadByIdFailsWithoutUserIdClaim()
        {
            var userId = Guid.NewGuid().ToString();
            var client = new TFactory().CreateClient();
            var createResult = await this.Create(
                client,
                userId);
            Assert.NotNull(createResult);
            var url = createResult.Links.First(link => link.Urn == $"urn:${this.UrnNamespace}:{Urn.ReadById}").Url;

            await client.AddApiKey(this.ApiKey)
            .AddToken(this.RequiredReadByIdRoles)
            .GetAsync<TReadResult>(
                url,
                HttpStatusCode.Unauthorized);
        }

        protected override IEnumerable<Claim> GetClaims(IEnumerable<Role> roles, string userId)
        {
            return roles.Select(
                    role => new Claim(
                        ClaimTypes.Role,
                        role.ToString()))
                .Append(
                    new Claim(
                        Constants.UserIdClaimType,
                        userId));
        }

        /*
        [Fact]
        public async Task DeleteFailsIfRoleIsMissing()
        {
            var (httpClient, url) = await this.GetValidDeleteUrl();
            await this.FailsIfRoleIsMissing(
                this.requiredDeleteRoles,
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
                this.requiredReadAllRoles,
                (client, url) => client.GetAsync<IEnumerable<TReadResult>>(
                    url,
                    HttpStatusCode.Forbidden));
        }

        [Fact]
        public async Task ReadByIdFailsIfRoleIsMissing()
        {
            await this.FailsIfRoleIsMissing(
                Urn.ReadById,
                this.requiredReadByIdRoles,
                (client, url) => client.GetAsync<TReadResult>(
                    url,
                    HttpStatusCode.Forbidden));
        }

        [Fact]
        public async Task UpdateFailsIfRoleIsMissing()
        {
            await this.FailsIfRoleIsMissing(
                Urn.Update,
                this.requiredUpdateRoles,
                (client, url) => client.PutAsync<TUpdate, TUpdateResult>(
                    url,
                    null,
                    HttpStatusCode.Forbidden));
        }*/
    }
}
