namespace WebApi.NetCore.Api.Models.Configuration
{
    using WebApi.NetCore.Api.Contracts.Configuration;

    /// <inheritdoc cref="IAppConfiguration" />
    public class AppConfiguration : IAppConfiguration
    {
        /// <summary>
        ///     Gets or sets the jwt configuration.
        /// </summary>
        public JwtConfiguration Jwt { get; set; } = new();

        /// <summary>
        ///     Gets or sets the jwt configuration.
        /// </summary>
        IJwtConfiguration IAppConfiguration.Jwt => this.Jwt;
    }
}
