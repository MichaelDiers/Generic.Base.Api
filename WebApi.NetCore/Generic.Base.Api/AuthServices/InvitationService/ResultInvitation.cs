namespace Generic.Base.Api.AuthServices.InvitationService
{
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Database;
    using Generic.Base.Api.Models;

    /// <summary>
    ///     Describes an invitation that is sent to the client.
    /// </summary>
    /// <seealso cref="Generic.Base.Api.Models.LinkResult" />
    public class ResultInvitation : LinkResult, IIdEntry
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ResultInvitation" /> class.
        /// </summary>
        /// <param name="id">The identifier of the invitation.</param>
        /// <param name="roles">The roles that assigned to the invitation.</param>
        /// <param name="links">The links that describes available operations.</param>
        public ResultInvitation(string id, IEnumerable<Role> roles, IEnumerable<Link> links)
            : base(links)
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
