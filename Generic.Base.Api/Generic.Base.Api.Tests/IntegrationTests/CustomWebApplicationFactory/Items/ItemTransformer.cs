namespace Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.Items
{
    using Generic.Base.Api.Models;
    using Generic.Base.Api.Transformer;

    internal class ItemTransformer
        : IControllerTransformer<Item, ResultItem>, IUserBoundAtomicTransformer<CreateItem, Item, UpdateItem>
    {
        /// <summary>
        ///     Transforms the specified entry of <typeparamref name="TEntry" /> to <typeparamref name="TResultEntry" />.
        /// </summary>
        /// <param name="entry">The data for creating an instance of <typeparamref name="TResultEntry" />.</param>
        /// <param name="links">The urn that describe the supported operations on the result entry.</param>
        /// <returns>The transformed entry of type <typeparamref name="TResultEntry" />.</returns>
        public ResultItem Transform(Item entry, IEnumerable<Link> links)
        {
            return new ResultItem(
                entry.Name,
                links);
        }

        /// <summary>
        ///     Transforms the specified <paramref name="createEntry" /> of type <typeparamref name="TCreateEntry" /> to
        ///     <typeparamref name="TEntry" />.
        /// </summary>
        /// <param name="createEntry">The data for creating an entry of type <typeparamref name="TEntry" />.</param>
        /// <param name="userId">The id of the owner.</param>
        /// <returns>The transformed entry of type <typeparamref name="TEntry" />.</returns>
        public Item Transform(CreateItem createEntry, string userId)
        {
            return new Item(
                Guid.NewGuid().ToString(),
                userId,
                createEntry.Name);
        }

        /// <summary>
        ///     Transforms the specified <paramref name="updateEntry" /> of type <typeparamref name="TUpdateEntry" /> to
        ///     <typeparamref name="TEntry" />.
        /// </summary>
        /// <param name="updateEntry">The data for creating an entry of type <typeparamref name="TEntry" />.</param>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="entryId">The id of the entry to be updated.</param>
        /// <returns>The transformed entry of type <typeparamref name="TEntry" />.</returns>
        public Item Transform(UpdateItem updateEntry, string userId, string entryId)
        {
            return new Item(
                entryId,
                userId,
                updateEntry.Name);
        }
    }
}
