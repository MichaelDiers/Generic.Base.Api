namespace WebApi.NetCore.Api.Tests.Integration
{
    using Generic.Base.Api.Jwt;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;

    internal class Factory : WebApplicationFactory<Program>
    {
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
                "foobar");

            builder.ConfigureTestServices(
                services =>
                {
                    var desc = services.FirstOrDefault(s => s.ServiceType == typeof(IJwtConfiguration));
                    if (desc is not null)
                    {
                        services.Remove(desc);
                    }

                    services.AddSingleton<IJwtConfiguration>(
                        _ => new JwtConfiguration(
                            "audience",
                            "issuer",
                            "GOOGLE_SECRET_MANAGER_JWT_KEY")
                        {
                            AccessTokenExpires = 1,
                            RefreshTokenExpires = 10,
                            Key = "Yh2k7QSu4l8CZg5p6X3Pna9L0Miy4D3Bvt0JVr87UcOj69Kqw5R2Nmf4FWs03Hdx"
                        });
                });
        }
    }
}
