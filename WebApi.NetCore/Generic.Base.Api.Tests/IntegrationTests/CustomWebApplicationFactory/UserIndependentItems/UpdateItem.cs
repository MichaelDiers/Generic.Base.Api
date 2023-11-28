namespace Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.UserIndependentItems

{
    /// <summary>
    ///     Describes the update data.
    /// </summary>
    public class UpdateItem
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UpdateItem" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
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
