using Generic.Base.Api.AuthServices.AuthService;
using Generic.Base.Api.AuthServices.InvitationService;
using Generic.Base.Api.AuthServices.TokenService;
using Generic.Base.Api.AuthServices.UserService;
using Generic.Base.Api.EnvironmentService;
using Generic.Base.Api.HashService;
using Generic.Base.Api.HealthChecks;
using Generic.Base.Api.Jwt;
using Generic.Base.Api.Middleware.ApiKey;
using Generic.Base.Api.Middleware.ErrorHandling;
using WebApi.NetCore.Api.Controllers;
using WebApi.NetCore.Api.Controllers.InvitationService;
using WebApi.NetCore.Api.Controllers.TokenEntryService;
using WebApi.NetCore.Api.Controllers.UserService;
using WebApi.NetCore.Api.Extensions;
using WebApi.NetCore.Api.HealthChecks;
using WebApi.NetCore.Api.Items;
using WebApi.NetCore.Api.Models.Configuration;
using TransactionHandler = WebApi.NetCore.Api.Controllers.TransactionHandler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencies().AddHashService().AddEnvironmentService();
builder.Services.AddCustomHealthChecks();
builder.AddJwtTokenService().AddApiKey();
builder.Services.AddUserService<object, TransactionHandler, UserProvider>();
builder.Services.AddInvitationService<object, TransactionHandler, InvitationProvider>();
builder.AddAuthService<object, TransactionHandler>();
builder.Services.AddTokenService<object, TransactionHandler, TokenEntryProvider>();

builder.Services.AddAuthServices<SignInDto, CreateUser, UserEntry, UpdateUser, object>(
    new AuthProvider(),
    new UserTransformer(),
    new TransactionHandler());

builder.Services.AddItemDependencies();

var appConfiguration = builder.Configuration.Get<AppConfiguration>();
builder.Services.AddConfiguration(appConfiguration);
var envEnvironment = builder.Services.AddEnvEnvironment(appConfiguration?.EnvNames);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenInfo();

builder.Services.AddCustomHealthChecks().AddCheck<DocumentationHealthCheck>(nameof(DocumentationHealthCheck));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
