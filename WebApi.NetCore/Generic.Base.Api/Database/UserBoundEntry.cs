namespace Generic.Base.Api.Database
{
    /// <summary>
    ///     An entry that is bound to an user.
    /// </summary>
    /// <seealso cref="IUserBoundEntry" />
    public class UserBoundEntry : IdEntry, IUserBoundEntry
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IdEntry" /> class.
        /// </summary>
        /// <param name="id">The identifier of the entry.</param>
        /// <param name="userId">The identifier of the user.</param>
        public UserBoundEntry(string id, string userId)
            : base(id)
        {
            this.UserId = userId;
        }

        /// <summary>
        ///     Gets the user identifier.
        /// </summary>
        public string UserId { get; }
    }
}
