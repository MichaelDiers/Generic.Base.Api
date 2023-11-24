namespace WebApi.NetCore.Api.Controllers
{
    using Generic.Base.Api.AuthServices;

    public class UpdateUser : IChangePassword
    {
        public string Password { get; set; }
        public string NewPassword { get; }
        public string OldPassword { get; }
    }
}
