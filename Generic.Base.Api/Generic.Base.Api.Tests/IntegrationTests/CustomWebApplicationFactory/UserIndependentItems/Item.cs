namespace Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.UserIndependentItems
{
    using Generic.Base.Api.Database;

    /// <summary>
    ///     Describes an item.
    /// </summary>
    public class Item : IdEntry
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IdEntry" /> class.
        /// </summary>
        /// <param name="id">The identifier of the entry.</param>
        /// <param name="name">The name of the item.</param>
        public Item(string id, string name)
            : base(id)
        {
            this.Name = name;
        }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public string Name { get; }
    }
}
