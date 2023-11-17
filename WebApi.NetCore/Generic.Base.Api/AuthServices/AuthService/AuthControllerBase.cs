namespace Generic.Base.Api.AuthServices.AuthService
{
    using System.Security.Claims;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Exceptions;
    using Generic.Base.Api.Jwt;
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Result;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    ///     Base for auth controllers.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    public abstract class AuthControllerBase : ControllerBase
    {
        /// <summary>
        /// The change password template.
        /// </summary>
        private const string ChangePasswordTemplate = "change-password";

        /// <summary>
        /// The refresh template.
        /// </summary>
        private const string RefreshTemplate = "refresh";

        /// <summary>
        /// The sign in template.
        /// </summary>
        private const string SignInTemplate = "sign-in";

        /// <summary>
        /// The sign up template.
        /// </summary>
        private const string SignUpTemplate = "sign-up";

        /// <summary>
        ///     The domain authentication service.
        /// </summary>
        private readonly IDomainAuthService domainAuthService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthControllerBase" /> class.
        /// </summary>
        /// <param name="domainAuthService">The domain authentication service.</param>
        protected AuthControllerBase(IDomainAuthService domainAuthService)
        {
            this.domainAuthService = domainAuthService;
        }

        /// <summary>
        ///     Delete the current user.
        /// </summary>
        /// <param name="signIn">The sign in data.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        [HttpDelete]
        [Authorize(Roles = nameof(Role.Accessor))]
        public async Task<ActionResult> Delete([FromBody] SignIn signIn, CancellationToken cancellationToken)
        {
            var claim = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claim is null)
            {
                throw new UnauthorizedException();
            }

            await this.domainAuthService.DeleteAsync(
                signIn,
                claim.Value,
                cancellationToken);
            return this.Ok();
        }

        /// <summary>
        ///     An options request for the available operations of the api.
        /// </summary>
        [HttpOptions]
        public LinkResult Options()
        {
            var baseUrl = this.Request.Path.Value;
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                return new LinkResult(Enumerable.Empty<ILink>());
            }

            return new LinkResult(
                new[]
                {
                    new Link(
                        Urn.Delete,
                        baseUrl),
                    new Link(
                        Urn.SignUp,
                        $"{baseUrl}/{AuthControllerBase.SignUpTemplate}"),
                    new Link(
                        Urn.SignIn,
                        $"{baseUrl}/{AuthControllerBase.SignInTemplate}"),
                    new Link(
                        Urn.ChangePassword,
                        $"{baseUrl}/{AuthControllerBase.ChangePasswordTemplate}"),
                    new Link(
                        Urn.Refresh,
                        $"{baseUrl}/{AuthControllerBase.RefreshTemplate}"),
                    new Link(
                        Urn.Options,
                        baseUrl)
                });
        }

        /// <summary>
        ///     Sign up a new user.
        /// </summary>
        /// <param name="signUp">The sign up data.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result are access and refresh tokens.</returns>
        [AllowAnonymous]
        [HttpPost(AuthControllerBase.SignUpTemplate)]
        public async Task<ActionResult<IToken>> Post([FromBody] SignUp signUp, CancellationToken cancellationToken)
        {
            var result = await this.domainAuthService.SignUpAsync(
                signUp,
                cancellationToken);
            return this.Ok(result);
        }

        /// <summary>
        ///     Sign in an existing user.
        /// </summary>
        /// <param name="signIn">The sign in data.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result are access and refresh tokens.</returns>
        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<ActionResult<IToken>> Post([FromBody] SignIn signIn, CancellationToken cancellationToken)
        {
            var result = await this.domainAuthService.SignInAsync(
                signIn,
                cancellationToken);
            return this.Ok(result);
        }

        /// <summary>
        ///     Change the password of an existing user.
        /// </summary>
        /// <param name="changePassword">The change password data.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result are access and refresh tokens.</returns>
        [HttpPost("change-password")]
        [Authorize(Roles = nameof(Role.Accessor))]
        public async Task<ActionResult> Post(
            [FromBody] ChangePassword changePassword,
            CancellationToken cancellationToken
        )
        {
            var claim = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claim is null)
            {
                throw new UnauthorizedException();
            }

            await this.domainAuthService.ChangePasswordAsync(
                changePassword,
                claim.Value,
                cancellationToken);
            return this.Ok();
        }

        /// <summary>
        ///     Refresh the access token by a given refresh token.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result are access and refresh tokens.</returns>
        [HttpPost("refresh")]
        [Authorize(Roles = nameof(Role.Refresher))]
        public async Task<ActionResult<IToken>> Post(CancellationToken cancellationToken)
        {
            var claim = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claim is null)
            {
                throw new UnauthorizedException();
            }

            var refreshToken = this.Request.Headers.Authorization.ToString().Replace("Bearer ", string.Empty);

            var result = await this.domainAuthService.RefreshAsync(
                refreshToken,
                claim.Value,
                cancellationToken);
            return this.Ok(result);
        }
    }
}
