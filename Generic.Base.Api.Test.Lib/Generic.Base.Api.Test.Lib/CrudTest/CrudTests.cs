namespace Generic.Base.Api.Test.Lib.CrudTest
{
    using System.Security.Claims;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Models;
    using Microsoft.AspNetCore.Mvc.Testing;

    public abstract class CrudTests<TEntryPoint, TFactory, TCreate, TCreateResult, TReadResult, TUpdate, TUpdateResult>
        : CrudTestsBase<TEntryPoint, TFactory, TCreate, TCreateResult, TReadResult, TUpdate, TUpdateResult>
        where TEntryPoint : class
        where TFactory : WebApplicationFactory<TEntryPoint>, new()
        where TCreate : class
        where TCreateResult : class, ILinkResult
        where TReadResult : class
        where TUpdate : class
        where TUpdateResult : class

    {
        protected CrudTests(
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
            : base(
                urnNamespace,
                entryPointUrl,
                apiKey,
                optionsRoles,
                requiredCreateRoles,
                requiredReadAllRoles,
                requiredReadByIdRoles,
                requiredUpdateRoles,
                requiredDeleteRoles)
        {
        }

        protected override bool RaiseDoubleCreateConflict => true;

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

        protected override IEnumerable<Claim> GetClaims(IEnumerable<Role> roles, string userId)
        {
            return roles.Select(
                role => new Claim(
                    ClaimTypes.Role,
                    role.ToString()));
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
    }
}
