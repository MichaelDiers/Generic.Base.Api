using Generic.Base.Api.HealthChecks;
using Generic.Base.Api.Jwt;
using Generic.Base.Api.Middleware.ApiKey;
using Generic.Base.Api.Middleware.ErrorHandling;
using Generic.Base.Api.MongoDb.AuthServices;
using Generic.Base.Api.MongoDb.Tests.IntegrationTests.CustomWebApplicationFactory;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks()
    .AddCheck<HealthCheckOk>(nameof(HealthCheckOk))
    .AddCheck<HealthCheckFail>(nameof(HealthCheckFail));
builder.AddJwtTokenService();
builder.AddApiKey();
builder.AddAuthServices();

builder.Services.AddControllers();

var app = builder.Build();
app.MapCustomHealthChecks();
app.UseErrorHandling();
app.UseApiKey();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
