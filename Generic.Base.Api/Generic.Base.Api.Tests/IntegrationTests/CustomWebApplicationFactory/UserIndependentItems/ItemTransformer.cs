namespace Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.UserIndependentItems
{
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Transformer;

    internal class ItemTransformer
        : IControllerTransformer<Item, ResultItem>, IAtomicTransformer<CreateItem, Item, UpdateItem>
    {
        /// <summary>
        ///     Transforms the specified <paramref name="createEntry" /> of type <see cref="CreateItem" /> to
        ///     <see cref="Item" />.
        /// </summary>
        /// <param name="createEntry">The data for creating an entry of type <see cref="Item" />.</param>
        /// <returns>The transformed entry of type <see cref="Item" />.</returns>
        public Item Transform(CreateItem createEntry)
        {
            return new Item(
                Guid.NewGuid().ToString(),
                createEntry.Name);
        }

        /// <summary>
        ///     Transforms the specified <paramref name="updateEntry" /> of type <see cref="UpdateItem" /> to
        ///     <see cref="Item" />.
        /// </summary>
        /// <param name="updateEntry">The data for creating an entry of type <see cref="Item" />.</param>
        /// <param name="entryId">The id of the entry to be updated.</param>
        /// <returns>The transformed entry of type <see cref="Item" />.</returns>
        public Item Transform(UpdateItem updateEntry, string entryId)
        {
            return new Item(
                entryId,
                updateEntry.Name);
        }

        /// <summary>
        ///     Transforms the specified entry of <see cref="Item" /> to <see cref="ResultItem" />.
        /// </summary>
        /// <param name="entry">The data for creating an instance of <see cref="ResultItem" />.</param>
        /// <param name="links">The urn that describe the supported operations on the result entry.</param>
        /// <returns>The transformed entry of type <see cref="ResultItem" />.</returns>
        public ResultItem Transform(Item entry, IEnumerable<Link> links)
        {
            return new ResultItem(
                entry.Name,
                links);
        }
    }
}
