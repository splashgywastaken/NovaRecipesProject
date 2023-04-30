using NovaRecipesProject.Services.RecipesSubscriptions.Jobs;
using Quartz;

namespace NovaRecipesProject.CommentsMailingJobScheduler.TaskScheduler;

public class TaskScheduler : ITaskScheduler
{
    private readonly ILogger<TaskScheduler> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TaskScheduler(
        ILogger<TaskScheduler> logger,
        IServiceScopeFactory serviceScopeFactory
        )
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public async void Start()
    {
        _logger.LogInformation("Task scheduler for CommentMailingJob job started");

        using var scope = _serviceScopeFactory.CreateScope();
        var schedulerFactory = scope.ServiceProvider.GetRequiredService<ISchedulerFactory>();
        var scheduler = await schedulerFactory.GetScheduler();

        var job = JobBuilder.Create<CommentsMailingJob>()
            .WithIdentity("commentMailingJob", "mailingJobs")
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity("commentMailingJobTrigger", "mailingTriggers")
            .StartNow()

            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(10)
                .RepeatForever())

            // Repeats every monday, wednesday, friday and sunday at 8 am
            //.WithCronSchedule("0 0 8 ? * MON,WED,FRI,SUN *")

            .Build();

        await scheduler.ScheduleJob(job, trigger);
        _logger.LogInformation("CommentMailingJob successfully scheduled");
    }
}