namespace WebApi.NetCore.Api.Controllers
{
    using Generic.Base.Api.AuthServices;

    public class UserEntry : IUser
    {
        public string Id { get; set; }
        public string Password { get; set; }
    }
}
