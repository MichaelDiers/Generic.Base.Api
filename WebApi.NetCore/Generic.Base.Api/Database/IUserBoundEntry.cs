namespace Generic.Base.Api.Database
{
    /// <summary>
    ///     An entry bound to an user.
    /// </summary>
    /// <seealso cref="Generic.Base.Api.Database.IIdEntry" />
    public interface IUserBoundEntry : IIdEntry
    {
        /// <summary>
        ///     Gets the user identifier.
        /// </summary>
        string UserId { get; }
    }
}
