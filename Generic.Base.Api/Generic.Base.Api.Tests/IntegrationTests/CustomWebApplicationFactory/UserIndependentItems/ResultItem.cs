namespace Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.UserIndependentItems
{
    using Generic.Base.Api.Models;

    public class ResultItem : LinkResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LinkResult" /> class.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="links">The links that describes available operations.</param>
        public ResultItem(string name, IEnumerable<Link> links)
            : base(links)
        {
            this.Name = name;
        }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public string Name { get; }
    }
}
