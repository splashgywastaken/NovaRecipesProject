using NovaRecipesProject.Api.Configuration;
using NovaRecipesProject.Services.Settings;
using NovaRecipesProject.Api;
using NovaRecipesProject.Api.Configuration;
using NovaRecipesProject.Context;
using NovaRecipesProject.Context.Setup;
using NovaRecipesProject.Services.Settings;
using NovaRecipesProject.Settings;

var builder = WebApplication.CreateBuilder(args);

// Getting settings 
var identitySettings = Settings.Load<IdentitySettings>("Identity");
var swaggerSettings = Settings.Load<SwaggerSettings>("Swagger");

// Add services to the container.
builder.AddAppLogger();

var services = builder.Services;

services.AddHttpContextAccessor();
services.AddAppCors();

services.AddAppDbContext();
//services.AddAppAuth(identitySettings);

services.AddAppVersioning();
services.AddAppHealthChecks();
services.AddAppSwagger(identitySettings, swaggerSettings);
services.AddAppAutoMappers();

services.AddAppControllerAndViews();

services.RegisterAppServices();

var app = builder.Build();

app.UseAppHealthChecks();

app.UseAppSwagger();

app.UseStaticFiles();

//app.UseAppAuth();

app.UseAppControllerAndViews();

app.UseAppMiddlewares();

DbInitializer.Execute(app.Services);
DbSeeder.Execute(app.Services, true, true);

app.Run();