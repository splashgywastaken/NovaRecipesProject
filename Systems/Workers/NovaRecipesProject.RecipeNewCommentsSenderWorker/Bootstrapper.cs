using NovaRecipesProject.Services.EmailSender;
using NovaRecipesProject.Services.RabbitMq;
using NovaRecipesProject.Worker;

namespace NovaRecipesProject.RecipeNewCommentsSenderWorker;

public static class Bootstrapper
{
    public static IServiceCollection RegisterAppServices(this IServiceCollection services)
    {
        services
            .AddRabbitMq()            
            .AddEmailSender()
            ;

        services.AddSingleton<ITaskExecutor, TaskExecutor>();

        return services;
    }
}
 



