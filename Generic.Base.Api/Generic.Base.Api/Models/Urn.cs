namespace Generic.Base.Api.Models
{
    /// <summary>
    ///     Describes the type of an operation.
    /// </summary>
    public enum Urn
    {
        /// <summary>
        ///     The undefined value.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        None = 0,

        /// <summary>
        ///     Operation for creating a new entry.
        /// </summary>
        Create,

        /// <summary>
        ///     Operation for reading all entries.
        /// </summary>
        ReadAll,

        /// <summary>
        ///     Operation for reading an entry by its id.
        /// </summary>
        ReadById,

        /// <summary>
        ///     Operation for updating an entry.
        /// </summary>
        Update,

        /// <summary>
        ///     Operation for deleting an entry.
        /// </summary>
        Delete,

        /// <summary>
        ///     Read the available operations.
        /// </summary>
        Options,

        /// <summary>
        ///     The sign up operation.
        /// </summary>
        SignUp,

        /// <summary>
        ///     The sign in operation.
        /// </summary>
        SignIn,

        /// <summary>
        ///     The change password operation.
        /// </summary>
        ChangePassword,

        /// <summary>
        ///     The token refresh operation.
        /// </summary>
        Refresh
    }
}
