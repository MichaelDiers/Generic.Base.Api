namespace WebApi.NetCore.Api.Controllers
{
    using Generic.Base.Api.AuthServices;

    public class SignInDto : IUser
    {
        public string Id { get; set; }
        public string Password { get; set; }
    }
}
