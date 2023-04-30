using NovaRecipesProject.CommentsMailingJobScheduler.TaskScheduler;
using NovaRecipesProject.Services.Actions;
using NovaRecipesProject.Services.RabbitMq;
using NovaRecipesProject.Services.RecipesSubscriptions;

namespace NovaRecipesProject.CommentsMailingJobScheduler;

using Services.EmailSender;

public static class Bootstrapper
{
    public static IServiceCollection RegisterAppServices(this IServiceCollection services)
    {
        services
            .AddScheduler()
            ;

        services.AddSingleton<ITaskScheduler, TaskScheduler.TaskScheduler>();

        return services;
    }
}
 



