namespace WebApi.NetCore.Api.Items
{
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Transformer;

    /// <summary>
    ///     Transformer for item entries.
    /// </summary>
    public class ItemTransformer : ITransformer<CreateItem, Item, UpdateItem, DatabaseItem, ItemResult>
    {
        /// <summary>
        ///     Transforms the specified create entry.
        /// </summary>
        /// <param name="createEntry">The create entry.</param>
        /// <returns>The transformed entry.</returns>
        public Item Transform(CreateItem createEntry)
        {
            return new Item(
                Guid.NewGuid().ToString(),
                createEntry.Name);
        }

        public Item Transform(UpdateItem updateItem, string id)
        {
            return new Item(
                id,
                updateItem.Name);
        }

        public ItemResult Transform(Item entry, IEnumerable<ILink> links)
        {
            return new ItemResult(
                entry,
                links);
        }

        /// <summary>
        ///     Transforms the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns>The transformed entry.</returns>
        public DatabaseItem Transform(Item entry)
        {
            return new DatabaseItem
            {
                Id = entry.Id,
                Name = entry.Name
            };
        }

        /// <summary>
        ///     Transforms the specified database entry.
        /// </summary>
        /// <param name="databaseEntry">The database entry.</param>
        /// <returns>The transformed entry.</returns>
        public Item Transform(DatabaseItem databaseEntry)
        {
            return new Item(
                databaseEntry.Id,
                databaseEntry.Name);
        }
    }
}
