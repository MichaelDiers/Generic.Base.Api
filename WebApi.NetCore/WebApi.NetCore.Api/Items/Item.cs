namespace WebApi.NetCore.Api.Items
{
    using WebApi.NetCore.Api.Models;

    /// <summary>
    ///     Describes an item.
    /// </summary>
    /// <seealso cref="IIdEntry" />
    public class Item : IdEntry
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Item" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        public Item(string id, string name)
            : base(id)
        {
            this.Name = name;
        }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}
