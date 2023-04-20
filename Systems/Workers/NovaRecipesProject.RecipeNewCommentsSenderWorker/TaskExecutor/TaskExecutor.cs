namespace NovaRecipesProject.Worker;

using Consts;
using RecipeNewCommentsSenderWorker;
using Services.EmailSender.Models;
using Services.EmailSender;
using Services.RabbitMq;

public class TaskExecutor : ITaskExecutor
{
    private readonly ILogger<TaskExecutor> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IRabbitMq _rabbitMq;

    public TaskExecutor(
        ILogger<TaskExecutor> logger,
        IServiceProvider serviceProvider,
        IRabbitMq rabbitMq
    )
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _rabbitMq = rabbitMq;
    }

    private async Task Execute<T>(Func<T, Task> action)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();

            var service = scope.ServiceProvider.GetService<T>();
            if (service != null)
                await action(service);
            else
                _logger.LogError($"Error: {action} wasn't resolved");
        }
        catch (Exception e)
        {
            _logger.LogError($"Error: {RabbitMqTaskQueueNames.SendNewRecipeCommentNotification}: {e.Message}");
            throw;
        }
    }

    // TODO: test later

    public void Start()
    {
        _rabbitMq.Subscribe<EmailModel>(RabbitMqTaskQueueNames.SendNewRecipeCommentNotification, async data
            => await Execute<IEmailSender>(async service =>
            {
                _logger.LogDebug($"RABBITMQ::: {RabbitMqTaskQueueNames.SendNewRecipeCommentNotification}: {data.Email} {data.Message}");
                await service.SendAsync(data);
            }));
    }
}