using WebApi.NetCore.Api.Extensions;
using WebApi.NetCore.Api.Middleware;
using WebApi.NetCore.Api.Models.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencies();

var appConfiguration = builder.Configuration.Get<AppConfiguration>();
builder.Services.AddConfiguration(appConfiguration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenInfo();

builder.Services.AddJwtAuthentication(appConfiguration?.Jwt);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
