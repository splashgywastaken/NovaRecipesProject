using NovaRecipesProject.RecipeInfoSenderWorker.TaskExecutor;
using NovaRecipesProject.Services.EmailSender;
using NovaRecipesProject.Services.RabbitMq;

namespace NovaRecipesProject.RecipeInfoSenderWorker;

public static class Bootstrapper
{
    public static IServiceCollection RegisterAppServices(this IServiceCollection services)
    {
        services
            .AddRabbitMq()            
            .AddEmailSender()
            ;

        services.AddSingleton<ITaskExecutor, TaskExecutor.TaskExecutor>();

        return services;
    }
}
 



