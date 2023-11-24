namespace WebApi.NetCore.Api.Controllers
{
    using Generic.Base.Api.AuthServices;

    public abstract class AuthController : GenericAuthController<SignInDto, SignUpDto, UpdateUser>
    {
        public AuthController(IDomainAuthService domainAuthService)
            : base(domainAuthService)
        {
        }
    }
}
