namespace WebApi.NetCore.Api.Contracts.Database
{
    /// <summary>
    ///     An entry with an id.
    /// </summary>
    public interface IIdEntry
    {
        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        string Id { get; }
    }
}
