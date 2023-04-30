using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace NovaRecipesProject.Services.RecipesSubscriptions.Factories;

/// <inheritdoc />
public class JobFactory : IJobFactory
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="serviceScopeFactory"></param>
    public JobFactory(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    /// <inheritdoc />
    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var job = (scope.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob)!;
        return job;
    }

    /// <inheritdoc />
    public void ReturnJob(IJob job)
    {
        
    }
}