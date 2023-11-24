namespace WebApi.NetCore.Api.Controllers
{
    using System.Security.Claims;
    using Generic.Base.Api.AuthServices;

    public class ClaimsUser : UserEntry, IClaimsUser
    {
        public IEnumerable<Claim> Claims { get; set; }
    }
}
