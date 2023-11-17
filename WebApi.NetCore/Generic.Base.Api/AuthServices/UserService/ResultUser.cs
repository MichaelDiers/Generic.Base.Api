namespace Generic.Base.Api.AuthServices.UserService
{
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Result;

    /// <summary>
    ///     Describes a user that is sent to the client.
    /// </summary>
    /// <seealso cref="LinkResult" />
    public class ResultUser : LinkResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ResultUser" /> class.
        /// </summary>
        /// <param name="id">The identifier of the user.</param>
        /// <param name="roles">The roles that are assigned to the user.</param>
        /// <param name="links">The links that describe available operations.</param>
        public ResultUser(string id, IEnumerable<Role> roles, IEnumerable<ILink> links)
            : base(links)
        {
            this.Id = id;
            this.Roles = roles;
        }

        /// <summary>
        ///     Gets the identifier of the user.
        /// </summary>
        public string Id { get; }

        /// <summary>
        ///     Gets the roles that are assigned to the user.
        /// </summary>
        public IEnumerable<Role> Roles { get; }
    }
}
