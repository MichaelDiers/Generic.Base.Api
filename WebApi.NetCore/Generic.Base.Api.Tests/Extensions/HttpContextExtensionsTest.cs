namespace Generic.Base.Api.Tests.Extensions
{
    using System.Net;
    using Generic.Base.Api.Extensions;
    using Microsoft.AspNetCore.Http;

    public class HttpContextExtensionsTest
    {
        [Fact]
        public async Task SetResponse()
        {
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent;
            const string message = "my message";

            var context = new DefaultHttpContext();

            await context.SetResponse(
                expectedStatusCode,
                message);

            Assert.Equal(
                "application/json; charset=utf-8",
                context.Response.ContentType);
            Assert.Equal(
                (int) expectedStatusCode,
                context.Response.StatusCode);
        }
    }
}
