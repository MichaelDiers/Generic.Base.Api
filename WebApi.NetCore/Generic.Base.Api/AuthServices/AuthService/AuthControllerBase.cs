namespace Generic.Base.Api.AuthServices.AuthService
{
    using System.Security.Claims;
    using Generic.Base.Api.Exceptions;
    using Generic.Base.Api.Jwt;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    ///     Base for auth controllers.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    public abstract class AuthControllerBase : ControllerBase
    {
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
        ///     Sign up a new user.
        /// </summary>
        /// <param name="signUp">The sign up data.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result are access and refresh tokens.</returns>
        [AllowAnonymous]
        [HttpPost("sign-up")]
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
        [AllowAnonymous]
        [HttpPost("change-password")]
        public async Task<ActionResult<IToken>> Post(
            [FromBody] ChangePassword changePassword,
            CancellationToken cancellationToken
        )
        {
            var claim = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claim is null)
            {
                throw new UnauthorizedException();
            }

            var result = await this.domainAuthService.ChangePasswordAsync(
                changePassword,
                claim.Value,
                cancellationToken);
            return this.Ok(result);
        }
    }
}
