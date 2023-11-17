namespace Generic.Base.Api.AuthServices.InvitationService
{
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Result;

    /// <summary>
    ///     Describes an invitation that is sent to the client.
    /// </summary>
    /// <seealso cref="Generic.Base.Api.Models.LinkResult" />
    public class ResultInvitation : LinkResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ResultInvitation" /> class.
        /// </summary>
        /// <param name="id">The identifier of the invitation.</param>
        /// <param name="roles">The roles that assigned to the invitation.</param>
        /// <param name="links">The links that describes available operations.</param>
        public ResultInvitation(string id, IEnumerable<Role> roles, IEnumerable<ILink> links)
            : base(links)
        {
            this.Id = id;
            this.Roles = roles;
        }

        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        public string Id { get; }

        /// <summary>
        ///     Gets the roles.
        /// </summary>
        public IEnumerable<Role> Roles { get; }
    }
}
