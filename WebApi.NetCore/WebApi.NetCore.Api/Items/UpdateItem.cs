namespace WebApi.NetCore.Api.Items
{
    public class UpdateItem
    {
        public UpdateItem(string name)
        {
            this.Name = name;
        }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public string Name { get; }
    }
}
