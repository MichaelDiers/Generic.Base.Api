namespace Generic.Base.Api.AuthServices.InvitationService
{
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Database;

    /// <summary>
    ///     Describes an invitation code that is required for a sign up operation.
    /// </summary>
    /// <seealso cref="Generic.Base.Api.Database.IIdEntry" />
    public class Invitation : IIdEntry
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Invitation" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roles">The roles.</param>
        public Invitation(string id, IEnumerable<Role> roles)
        {
            this.Id = id;
            this.Roles = roles;
        }

        /// <summary>
        ///     Gets the roles.
        /// </summary>
        public IEnumerable<Role> Roles { get; }

        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        public string Id { get; }
    }
}
