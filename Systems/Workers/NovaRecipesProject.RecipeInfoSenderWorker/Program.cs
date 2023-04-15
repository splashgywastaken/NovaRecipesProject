using NovaRecipesProject.Context;
using NovaRecipesProject.RecipeInfoSenderWorker;
using NovaRecipesProject.RecipeInfoSenderWorker.Configuration;
using NovaRecipesProject.RecipeInfoSenderWorker.TaskExecutor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddAppLogger();

// Configure services
var services = builder.Services;

services.AddHttpContextAccessor();

services.AddAppDbContext(builder.Configuration);

services.AddAppHealthChecks();

services.RegisterAppServices();


// Configure the HTTP request pipeline.

var app = builder.Build();

app.UseAppHealthChecks();


// Start task executor

app.Services.GetRequiredService<ITaskExecutor>().Start();


// Run app

app.Run();