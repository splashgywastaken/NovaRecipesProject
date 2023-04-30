using NovaRecipesProject.CommentsMailingJobScheduler;
using NovaRecipesProject.CommentsMailingJobScheduler.Configuration;
using NovaRecipesProject.CommentsMailingJobScheduler.TaskScheduler;
using NovaRecipesProject.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddAppLogger();

// Configure services
var services = builder.Services;

services.AddHttpContextAccessor();

services.AddAppDbContext(builder.Configuration);

services.AddAppHealthChecks();

services.RegisterAppServices();

var app = builder.Build();

app.UseAppHealthChecks();


// Start task scheduler
app.Services.GetRequiredService<ITaskScheduler>().Start();


// Run app
app.Run();