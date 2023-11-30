using Generic.Base.Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var app = WebApplication.Create(args);

app.MapDelete(
    "/{statusCode:int}",
    Results.StatusCode);

app.MapDelete(
    "/errorResult/{statusCode:int}",
    (int statusCode) => Results.Json(
        new ErrorResult("error"),
        statusCode: statusCode));

app.MapGet(
    "/{dataFromClient}/{statusCode:int}",
    (string dataFromClient, int statusCode) => Results.Json(
        dataFromClient,
        statusCode: statusCode));

app.MapPost(
    "/{dataFromClient}/{statusCode:int}",
    (string dataFromClient, int statusCode) => Results.Json(
        dataFromClient,
        statusCode: statusCode));

app.MapPut(
    "/{dataFromClient}/{statusCode:int}",
    (string dataFromClient, int statusCode) => Results.Json(
        dataFromClient,
        statusCode: statusCode));

app.MapMethods(
    "/{dataFromClient}/{statusCode:int}",
    new[] {HttpMethod.Options.ToString()},
    (string dataFromClient, int statusCode) => Results.Json(
        dataFromClient,
        statusCode: statusCode));

app.Run();

public partial class Program
{
}
