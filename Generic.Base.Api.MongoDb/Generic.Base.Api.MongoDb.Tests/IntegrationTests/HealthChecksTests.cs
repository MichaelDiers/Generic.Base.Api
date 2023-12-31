﻿namespace Generic.Base.Api.MongoDb.Tests.IntegrationTests
{
    using System.Net;
    using Generic.Base.Api.MongoDb.Tests.IntegrationTests.CustomWebApplicationFactory;
    using Newtonsoft.Json.Linq;

    [Trait(
        "TestType",
        "MongoDbIntegrationTest")]
    public class HealthChecksTests
    {
        [Fact]
        public async Task HealthCheck()
        {
            var client = TestFactory.GetClient().AddApiKey();

            var response = await client.GetAsync("/health");

            Assert.Equal(
                HttpStatusCode.ServiceUnavailable,
                response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(jsonString);
            Assert.NotNull(json);

            var status = json.GetValue("status")?.Value<string>();
            Assert.Equal(
                "Unhealthy",
                status);

            var healthy = json.GetValue("healthy")?.Value<string>();
            Assert.Equal(
                "1/2",
                healthy);

            var results = json.GetValue("results")?.Value<JArray>();
            Assert.NotNull(results);
            Assert.Equal(
                2,
                results.Count);

            foreach (var checkResult in Enumerable.Range(
                             0,
                             2)
                         .Select(i => results[i].Value<JObject>()))
            {
                Assert.NotNull(checkResult);
                var data = new Dictionary<string, string>();
                foreach (var key in new[]
                         {
                             "check",
                             "status",
                             "description"
                         })
                {
                    var value = checkResult.GetValue(key)?.Value<string>();
                    Assert.NotNull(value);
                    data[key] = value;
                }

                switch (data["check"])
                {
                    case nameof(HealthCheckOk):
                        Assert.Equal(
                            "Healthy",
                            data["status"]);
                        Assert.Equal(
                            nameof(HealthCheckOk),
                            data["description"]);
                        break;
                    case nameof(HealthCheckFail):
                        Assert.Equal(
                            "Unhealthy",
                            data["status"]);
                        Assert.Equal(
                            nameof(HealthCheckFail),
                            data["description"]);
                        break;
                    default:
                        Assert.Fail("Should not be executed.");
                        break;
                }
            }
        }
    }
}
