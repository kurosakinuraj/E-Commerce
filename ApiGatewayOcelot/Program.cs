using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using JwtAuthManager;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddCustomJwtAuthentication();
var app = builder.Build();
await app.UseOcelot();

//authentication
app.UseAuthentication();
app.UseAuthorization();

app.Run();
