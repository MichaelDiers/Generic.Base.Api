namespace WebApi.NetCore.Api.Items
{
    using WebApi.NetCore.Api.Models;

    /// <summary>
    ///     Describes an item stored in database.
    /// </summary>
    /// <seealso cref="DatabaseEntry" />
    public class DatabaseItem : DatabaseEntry
    {
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
