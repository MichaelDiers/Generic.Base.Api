namespace WebApi.NetCore.Api.Items
{
    using Generic.Base.Api.Models;

    public class ItemResult : Item, ILinkResult

    {
        public ItemResult(Item item, IEnumerable<ILink> links)
            : this(
                item.Id,
                item.Name,
                links)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Item" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        public ItemResult(string id, string name, IEnumerable<ILink> links)
            : base(
                id,
                name)
        {
            this.Links = links;
        }

        public IEnumerable<ILink> Links { get; }
    }
}
