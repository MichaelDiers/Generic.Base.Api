namespace WebApi.NetCore.Api.Controllers
{
    using System.Security.Claims;
    using Generic.Base.Api.AuthServices;
    using Generic.Base.Api.Transformer;

    public class UserTransformer : IAtomicTransformer<IUser, IClaimsUser, UpdateUser>
    {
        public IClaimsUser Transform(IUser createEntry)
        {
            return new ClaimsUser
            {
                Id = createEntry.Id,
                Password = createEntry.Password,
                Claims = new[]
                {
                    new Claim(
                        ClaimTypes.Role,
                        "User")
                }
            };
        }

        /// <summary>
        ///     Transforms the specified <paramref name="updateEntry" /> of type <typeparamref name="TUpdateEntry" /> to
        ///     <typeparamref name="TEntry" />.
        /// </summary>
        /// <param name="updateEntry">The data for creating an entry of type <typeparamref name="TEntry" />.</param>
        /// <param name="id">The id of the entry to be updated.</param>
        /// <returns>The transformed entry of type <typeparamref name="TEntry" />.</returns>
        public IClaimsUser Transform(UpdateUser updateEntry, string id)
        {
            return new ClaimsUser
            {
                Id = id,
                Password = updateEntry.Password,
                Claims = Enumerable.Empty<Claim>()
            };
        }
    }
}
