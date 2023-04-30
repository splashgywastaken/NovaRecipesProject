using Microsoft.Extensions.DependencyInjection;
using NovaRecipesProject.Services.Actions;
using NovaRecipesProject.Services.RabbitMq;
using NovaRecipesProject.Services.RecipesSubscriptions.Factories;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace NovaRecipesProject.Services.RecipesSubscriptions;

/// <summary>
/// Bootstrapper for DI for Scheduler service
/// </summary>
public static class Bootstrapper
{
    /// <summary>
    /// Adds scheduler to app using Quartz.Net
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddScheduler(this IServiceCollection services)
    {
        services.AddTransient<ISchedulerFactory, StdSchedulerFactory>()
            .AddTransient<IJobFactory, JobFactory>()
            .AddRabbitMq()
            .AddActions();

        return services;
    }
}