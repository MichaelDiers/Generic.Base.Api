namespace Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.UserIndependentItems
{
    public class CreateItem
    {
        public CreateItem(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
