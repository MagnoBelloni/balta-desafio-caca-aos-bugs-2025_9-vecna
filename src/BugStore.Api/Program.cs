using BugStore.Api;
using BugStore.Api.Controllers;
using BugStore.Application;
using BugStore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiDependencies();
builder.Services.AddApplicationDependencies();
builder.Services.AddInfrastructureDependencies(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapCustomersEndpoints();
app.MapProductsEndpoints();
app.MapOrdersEndpoints();
app.MapReportsEndpoints();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BugStore API V1");
    c.RoutePrefix = string.Empty;
});

app.Run();
