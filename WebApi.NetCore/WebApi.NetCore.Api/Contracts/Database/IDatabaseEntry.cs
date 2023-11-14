namespace WebApi.NetCore.Api.Contracts.Database
{
    /// <summary>
    ///     Describes a database entry.
    /// </summary>
    /// <seealso cref="WebApi.NetCore.Api.Contracts.Database.IIdEntry" />
    public interface IDatabaseEntry : IIdEntry
    {
        /// <summary>
        ///     Gets the id of the entry that is used in the application.
        /// </summary>
        string ApplicationId { get; }
    }
}
