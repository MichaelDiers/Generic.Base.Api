namespace Generic.Base.Api.AuthServices.UserService
{
    using Generic.Base.Api.Database;

    /// <summary>
    ///     Describes the user data.
    /// </summary>
    /// <seealso cref="IIdEntry" />
    public class User : IIdEntry
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="User" /> class.
        /// </summary>
        /// <param name="id">The identifier of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="roles">The roles that are assigned to the user.</param>
        public User(string id, string password, IEnumerable<Role> roles)
        {
            this.Id = id;
            this.Password = password;
            this.Roles = roles.ToArray();
        }

        /// <summary>
        ///     Gets the password.
        /// </summary>
        public string Password { get; }

        /// <summary>
        ///     Gets the roles that are assigned to the user.
        /// </summary>
        public IEnumerable<Role> Roles { get; }

        /// <summary>
        ///     Gets the identifier of the user.
        /// </summary>
        public string Id { get; }
    }
}
