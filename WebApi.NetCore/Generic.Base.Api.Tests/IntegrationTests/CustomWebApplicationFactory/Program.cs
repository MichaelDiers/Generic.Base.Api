using Generic.Base.Api.AuthServices;
using Generic.Base.Api.AuthServices.InvitationService;
using Generic.Base.Api.AuthServices.TokenService;
using Generic.Base.Api.AuthServices.UserService;
using Generic.Base.Api.Database;
using Generic.Base.Api.Extensions;
using Generic.Base.Api.HealthChecks;
using Generic.Base.Api.Jwt;
using Generic.Base.Api.Middleware.ApiKey;
using Generic.Base.Api.Middleware.ErrorHandling;
using Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory;
using Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.Items;
using Generic.Base.Api.Transformer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks()
    .AddCheck<HealthCheckOk>(nameof(HealthCheckOk))
    .AddCheck<HealthCheckFail>(nameof(HealthCheckFail));
builder.AddJwtTokenService();
builder.AddApiKey();
builder
    .AddAuthServices<object, TransactionHandler, InMemoryProvider<Invitation, object>,
        InMemoryProvider<TokenEntry, object>, InMemoryProvider<User, object>>();

builder.Services.AddUserBoundServices<CreateItem, Item, UpdateItem, object>();
builder.Services.TryAddScoped<IControllerTransformer<Item, ResultItem>, ItemTransformer>();
builder.Services.TryAddScoped<ITransactionHandler<object>, TransactionHandler>();
builder.Services.TryAddScoped<IUserBoundProvider<Item, object>, InMemoryUserBoundProvider<Item, object>>();
builder.Services.TryAddScoped<IUserBoundAtomicTransformer<CreateItem, Item, UpdateItem>, ItemTransformer>();

builder.Services
    .AddServices<Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.UserIndependentItems.CreateItem,
        Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.UserIndependentItems.Item, Generic.Base.Api.
        Tests.IntegrationTests.CustomWebApplicationFactory.UserIndependentItems.UpdateItem, object>();
builder.Services
    .TryAddScoped<
        IControllerTransformer<
            Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.UserIndependentItems.Item,
            Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.UserIndependentItems.ResultItem>,
        Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.UserIndependentItems.ItemTransformer>();
builder.Services
    .TryAddScoped<
        IProvider<Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.UserIndependentItems.Item, object>
        , InMemoryProvider<Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.UserIndependentItems.Item
            , object>>();
builder.Services
    .TryAddScoped<
        IAtomicTransformer<
            Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.UserIndependentItems.CreateItem,
            Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.UserIndependentItems.Item,
            Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.UserIndependentItems.UpdateItem>,
        Generic.Base.Api.Tests.IntegrationTests.CustomWebApplicationFactory.UserIndependentItems.ItemTransformer>();

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

public partial class Program
{
}
