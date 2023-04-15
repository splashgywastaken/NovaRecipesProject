using NovaRecipesProject.Consts;
using NovaRecipesProject.Services.EmailSender;
using NovaRecipesProject.Services.EmailSender.Models;
using NovaRecipesProject.Services.RabbitMq;

namespace NovaRecipesProject.RecipeInfoSenderWorker.TaskExecutor;

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
            _logger.LogError($"Error: {RabbitMqTaskQueueNames.SendRecipesInfoEmail}: {e.Message}");
            throw;
        }
    }

    public void Start()
    {
        _rabbitMq.Subscribe<EmailModel>(RabbitMqTaskQueueNames.SendRecipesInfoEmail, async data
            => await Execute<IEmailSender>(async service =>
            {
                _logger.LogDebug($"RABBITMQ::: {RabbitMqTaskQueueNames.SendRecipesInfoEmail}: {data.Email} {data.Message}");
                await service.SendAsync(data);
            }));
    }
}