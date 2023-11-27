namespace Generic.Base.Api.Database
{
    /// <inheritdoc cref="IIdEntry" />
    public class IdEntry : IIdEntry
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IdEntry" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public IdEntry(string id)
        {
            this.Id = id;
        }

        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        public string Id { get; }
    }
}
