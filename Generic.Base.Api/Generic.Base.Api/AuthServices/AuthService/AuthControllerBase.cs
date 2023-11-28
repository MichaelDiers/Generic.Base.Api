namespace Generic.Base.Api.AuthServices.AuthService
{
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Exceptions;
    using Generic.Base.Api.Extensions;
    using Generic.Base.Api.Jwt;
    using Generic.Base.Api.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    ///     Base for auth controllers.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    public abstract class AuthControllerBase : ControllerBase
    {
        /// <summary>
        ///     The change password template.
        /// </summary>
        private const string ChangePasswordTemplate = "change-password";

        /// <summary>
        ///     The refresh template.
        /// </summary>
        private const string RefreshTemplate = "refresh";

        /// <summary>
        ///     The sign in template.
        /// </summary>
        private const string SignInTemplate = "sign-in";

        /// <summary>
        ///     The sign up template.
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
            if (!this.User.Claims.TryGetUserId(out var userId))
            {
                throw new UnauthorizedException();
            }

            await this.domainAuthService.DeleteAsync(
                signIn,
                userId,
                cancellationToken);
            return this.Ok();
        }

        /// <summary>
        ///     An options request for the available operations of the api.
        /// </summary>
        [HttpOptions]
        [AllowAnonymous]
        public ILinkResult Options()
        {
            var baseUrl = this.Request.Path.Value;
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                return new LinkResult(Enumerable.Empty<Link>());
            }

            var links = new[]
            {
                ClaimLink.Create(
                    this.GetType().Name[..^10],
                    Urn.Delete,
                    baseUrl,
                    Role.Accessor),
                ClaimLink.Create(
                    this.GetType().Name[..^10],
                    Urn.SignUp,
                    $"{baseUrl}/{AuthControllerBase.SignUpTemplate}"),
                ClaimLink.Create(
                    this.GetType().Name[..^10],
                    Urn.SignIn,
                    $"{baseUrl}/{AuthControllerBase.SignInTemplate}"),
                ClaimLink.Create(
                    this.GetType().Name[..^10],
                    Urn.ChangePassword,
                    $"{baseUrl}/{AuthControllerBase.ChangePasswordTemplate}",
                    Role.Accessor),
                ClaimLink.Create(
                    this.GetType().Name[..^10],
                    Urn.Refresh,
                    $"{baseUrl}/{AuthControllerBase.RefreshTemplate}",
                    Role.Refresher),
                ClaimLink.Create(
                    this.GetType().Name[..^10],
                    Urn.Options,
                    baseUrl)
            };

            return new LinkResult(links.Where(link => link.CanBeAccessed(this.User.Claims)));
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
            if (!this.User.Claims.TryGetUserId(out var userId))
            {
                throw new UnauthorizedException();
            }

            var token = this.Request.Headers.Authorization.ToString()[7..];

            var tokens = await this.domainAuthService.ChangePasswordAsync(
                changePassword,
                userId,
                token,
                cancellationToken);
            return this.Ok(tokens);
        }

        /// <summary>
        ///     Refresh the access token by a given refresh token.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result are access and refresh tokens.</returns>
        [HttpGet("refresh")]
        [Authorize(Roles = nameof(Role.Refresher))]
        public async Task<ActionResult<IToken>> Post(CancellationToken cancellationToken)
        {
            if (!this.User.Claims.TryGetUserId(out var userId))
            {
                throw new UnauthorizedException();
            }

            var refreshToken = this.Request.Headers.Authorization.ToString()[7..];

            var result = await this.domainAuthService.RefreshAsync(
                refreshToken,
                userId,
                cancellationToken);
            return this.Ok(result);
        }
    }
}
