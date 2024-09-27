using Microsoft.AspNetCore.Identity;
using Common.Entities;

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services); // calling ConfigureServices method
var app = builder.Build();
app.MapIdentityApi<ApplicationUser>();
startup.Configure(app, builder.Environment); // calling Configure method