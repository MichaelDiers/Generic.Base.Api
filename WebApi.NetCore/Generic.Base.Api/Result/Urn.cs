namespace Generic.Base.Api.Result
{
    /// <summary>
    ///     Describes the type of an operation.
    /// </summary>
    public enum Urn
    {
        /// <summary>
        ///     The undefined value.
        /// </summary>
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
        Options
    }
}
