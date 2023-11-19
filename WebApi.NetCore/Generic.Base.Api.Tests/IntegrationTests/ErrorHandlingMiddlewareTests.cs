namespace Generic.Base.Api.Tests.IntegrationTests
{
    using System.Net;
    using Generic.Base.Api.Exceptions;
    using Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory;

    public class ErrorHandlingMiddlewareTests
    {
        [Theory]
        [InlineData(
            nameof(BadRequestException),
            HttpStatusCode.BadRequest)]
        [InlineData(
            nameof(ConflictException),
            HttpStatusCode.Conflict)]
        [InlineData(
            nameof(NotFoundException),
            HttpStatusCode.NotFound)]
        [InlineData(
            nameof(UnauthorizedException),
            HttpStatusCode.Unauthorized)]
        [InlineData(
            nameof(ArgumentException),
            HttpStatusCode.InternalServerError)]
        [InlineData(
            nameof(ArgumentNullException),
            HttpStatusCode.InternalServerError)]
        public async Task ErrorHandlingCheck(string exception, HttpStatusCode statusCode)
        {
            var client = TestFactory.GetClientWithApiKey();

            var response = await client.GetAsync($"api/ErrorHandlingMiddleware/{exception}");

            Assert.Equal(
                statusCode,
                response.StatusCode);
        }
    }
}
