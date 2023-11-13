// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.NetCore.Api.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using WebApi.NetCore.Api.Contracts;
    using WebApi.NetCore.Api.Contracts.Services;

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [Authorize(Roles = nameof(Role.Admin))]
        [HttpPost("apiKeyAdmin")]
        public ActionResult ApiKeyAdmin()
        {
            return this.Ok();
        }

        [AllowAnonymous]
        [HttpPost("apiKeyAllowAnonymous")]
        public ActionResult ApiKeyAllowAnonymous()
        {
            return this.Ok();
        }

        [Authorize(Roles = nameof(Role.Admin))]
        [HttpPost("requiresAdminRole")]
        public ActionResult RequiresAdminRole()
        {
            return this.Ok();
        }

        [Authorize(Roles = $"{nameof(Role.Admin)},{nameof(Role.User)}")]
        [HttpPost("requiresAdminRoleOrUserRole")]
        public ActionResult RequiresAdminRoleOrUserRole()
        {
            return this.Ok();
        }

        [HttpPost("requiresToken")]
        public ActionResult RequiresToken()
        {
            return this.Ok();
        }

        [Authorize(Roles = nameof(Role.User))]
        [HttpPost("requiresUserRole")]
        public ActionResult RequiresUserRole()
        {
            return this.Ok();
        }

        [AllowAnonymous]
        [HttpPost("signInAs/{role:int}")]
        public string SignInAsync([FromRoute] int role)
        {
            return this.authService.SignIn((Role) role);
        }

        [AllowAnonymous]
        [HttpPost("signInAs/{role1:int}/{role2:int}")]
        public string SignInAsync([FromRoute] int role1, [FromRoute] int role2)
        {
            return this.authService.SignIn(
                (Role) role1,
                (Role) role2);
        }
    }
}
