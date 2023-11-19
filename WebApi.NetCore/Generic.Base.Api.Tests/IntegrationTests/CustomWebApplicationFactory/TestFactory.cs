﻿namespace Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory
{
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
        public static readonly string ApiKey = Guid.NewGuid().ToString();

        /// <summary>
        ///     Gets the client.
        /// </summary>
        /// <param name="addApiKey">if set to <c>true</c> add the x-api-key; otherwise omit the header.</param>
        /// <returns>A <see cref="HttpClient" /> for application request.</returns>
        public static HttpClient GetClient(bool addApiKey = false)
        {
            var client = new TestFactory().CreateClient();
            if (addApiKey)
            {
                client.DefaultRequestHeaders.Add(
                    "x-api-key",
                    TestFactory.ApiKey);
            }

            return client;
        }

        /// <summary>
        ///     Gets the client and sets the x-api-key header.
        /// </summary>
        /// <returns>A <see cref="HttpClient" />.</returns>
        public static HttpClient GetClientWithApiKey()
        {
            return TestFactory.GetClient(true);
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
                "X_API_KEY",
                TestFactory.ApiKey);
        }
    }
}