namespace Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.Items
{
    using Generic.Base.Api.Database;

    /// <summary>
    ///     Describes an item.
    /// </summary>
    /// <seealso cref="Generic.Base.Api.Database.UserBoundEntry" />
    public class Item : UserBoundEntry
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IdEntry" /> class.
        /// </summary>
        /// <param name="id">The identifier of the entry.</param>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="name">The name of the item.</param>
        public Item(string id, string userId, string name)
            : base(
                id,
                userId)
        {
            this.Name = name;
        }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public string Name { get; }
    }
}
