namespace Generic.Base.Api.MongoDb.Tests.IntegrationTests.CustomWebApplicationFactory
{
    using Generic.Base.Api.MongoDb.AuthServices;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;

    /// <summary>
    ///     A <see cref="WebApplicationFactory{TEntryPoint}" /> that setups the application.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory&lt;Program&gt;" />
    internal class TestFactory : WebApplicationFactory<Program>
    {
        /// <summary>
        ///     The API key used for requests.
        /// </summary>
        public const string ApiKey = "the api key";

        /// <summary>
        ///     The entry point of the application.
        /// </summary>
        public const string EntryPointUrl = "/api/Options";

        /// <summary>
        ///     Gets the client.
        /// </summary>
        /// <returns>A <see cref="HttpClient" /> for application request.</returns>
        public static HttpClient GetClient()
        {
            return new TestFactory().CreateClient();
        }

        /// <summary>
        ///     Gives a fixture an opportunity to configure the application before it gets built.
        /// </summary>
        /// <param name="builder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> for the application.</param>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            Environment.SetEnvironmentVariable(
                "GOOGLE_SECRET_MANAGER_JWT_KEY",
                "Yh2k7QSu4l8CZg5p6X3Pna9L0Miy4D3Bvt0JVr87UcOj69Kqw5R2Nmf4FWs03Hdx");
            Environment.SetEnvironmentVariable(
                AuthServicesDependencies.MongoDbConnectionStringKey,
                // ReSharper disable once StringLiteralTypo
                "mongodb://localhost:27017/?replicaSet=warehouse_replSet");
            Environment.SetEnvironmentVariable(
                "X_API_KEY",
                TestFactory.ApiKey);
        }
    }
}
