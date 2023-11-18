using Generic.Base.Api.AuthServices;
using Generic.Base.Api.AuthServices.InvitationService;
using Generic.Base.Api.AuthServices.TokenService;
using Generic.Base.Api.AuthServices.UserService;
using Generic.Base.Api.HealthChecks;
using Generic.Base.Api.Jwt;
using Generic.Base.Api.Middleware.ApiKey;
using Generic.Base.Api.Middleware.ErrorHandling;
using Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks()
    .AddCheck<HealthCheckOk>(nameof(HealthCheckOk))
    .AddCheck<HealthCheckFail>(nameof(HealthCheckFail));
builder.AddJwtTokenService();
builder
    .AddAuthServices<object, TransactionHandler, InMemoryProvider<Invitation, object>,
        InMemoryProvider<TokenEntry, object>, InMemoryProvider<User, object>>();
builder.AddApiKey();
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
