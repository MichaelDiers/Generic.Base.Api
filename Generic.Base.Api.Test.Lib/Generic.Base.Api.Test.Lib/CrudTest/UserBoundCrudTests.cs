namespace Generic.Base.Api.Test.Lib.CrudTest
{
    using System.Net;
    using System.Security.Claims;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Test.Lib.Extensions;
    using Microsoft.AspNetCore.Mvc.Testing;

    /// <summary>
    ///     Base class for user bound crud tests.
    /// </summary>
    /// <typeparam name="TEntryPoint">The type of the entry point.</typeparam>
    /// <typeparam name="TFactory">The type of the factory.</typeparam>
    /// <typeparam name="TCreate">The type of the data for creating a entry.</typeparam>
    /// <typeparam name="TCreateResult">The type of the create result.</typeparam>
    /// <typeparam name="TReadResult">The type of the read result.</typeparam>
    /// <typeparam name="TUpdate">The type of the data for updating an entry.</typeparam>
    /// <typeparam name="TUpdateResult">The type of the update result.</typeparam>
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
        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="UserBoundCrudTests{TEntryPoint, TFactory, TCreate, TCreateResult, TReadResult, TUpdate, TUpdateResult}" />
        ///     class.
        /// </summary>
        /// <param name="apiKey">The valid API key.</param>
        protected UserBoundCrudTests(string apiKey)
            : base(apiKey)
        {
        }

        /// <summary>
        ///     Gets a value indicating whether a double create raises a conflict.
        /// </summary>
        /// <value>
        ///     <c>true</c> if a double create raises a conflict; otherwise, <c>false</c>.
        /// </value>
        protected override bool RaiseDoubleCreateConflict => false;

        /// <summary>
        ///     Creating an entry fails without a user id claim.
        /// </summary>
        [Fact]
        public async Task CreateFailsWithoutUserIdClaim()
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
                .AddToken(this.RequiredCreateRoles)
                .PostAsync<TCreate, TCreateResult>(
                    url,
                    this.GetValidCreateEntry(),
                    HttpStatusCode.Unauthorized);
        }

        /// <summary>
        ///     Reads an entry fails without a user id claim.
        /// </summary>
        [Fact]
        public async Task ReadByIdFailsWithoutUserIdClaim()
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

            await client.AddApiKey(this.ApiKey)
            .AddToken(this.RequiredReadByIdRoles)
            .GetAsync<TReadResult>(
                url,
                HttpStatusCode.Unauthorized);
        }

        /// <summary>
        ///     Gets the claims depending on the given roles and the user id.
        /// </summary>
        /// <param name="roles">The roles that are added to the result claims.</param>
        /// <param name="userId">The user identifier that is added as a claim.</param>
        /// <returns>The created claims.</returns>
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
    }
}
