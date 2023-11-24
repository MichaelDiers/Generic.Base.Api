namespace WebApi.NetCore.Api.Items
{
    /// <summary>
    ///     Describes the data for creating an item.
    /// </summary>
    public class CreateItem
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateItem" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public CreateItem(string name)
        {
            this.Name = name;
        }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}
