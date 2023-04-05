using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace NovaRecipesProject.Services.RabbitMq;

/// <summary>
/// Main RabbitMQ class used to subscribe, push and publish
/// </summary>
public class RabbitMq : IRabbitMq, IDisposable
{
    private readonly object _connectionLock = new();
    private readonly RabbitMqSettings _settings;
    private IModel _channel = null!;

    private IConnection _connection = null!;

    /// <summary>
    /// Contructor for rabbitmq 
    /// </summary>
    /// <param name="settings"></param>
    public RabbitMq(RabbitMqSettings settings)
    {
        _settings = settings;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Custom dispose method used with GC.SuppressFinalize(this) to evade CA1816 error
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        _channel?.Close();
        _connection?.Close();
    }

    private IModel GetChannel()
    {
        return _channel;
    }

    private Task RegisterListener(string queueName, EventHandler<BasicDeliverEventArgs> onReceive, int? messageLifetime = null)
    {
        Connect();

        AddQueue(queueName, messageLifetime);

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += onReceive;

        _channel.BasicConsume(queueName, false, consumer);
        return Task.CompletedTask;
    }

    private Task Publish<T>(string queueName, T data)
    {
        Connect();

        AddQueue(queueName);

        if (data == null) return Task.CompletedTask;
        var json = JsonSerializer.Serialize<object>(data, new JsonSerializerOptions() { });

        var message = Encoding.UTF8.GetBytes(json);

        _channel.BasicPublish(string.Empty, queueName, null, message);

        return Task.CompletedTask;
    }

    private void Connect()
    {
        lock (_connectionLock)
        {
            if (_connection?.IsOpen ?? false)
                return;

            var factory = new ConnectionFactory
            {
                Uri = new Uri(_settings.Uri),
                UserName = _settings.UserName,
                Password = _settings.Password,

                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(5)
            };

            var retriesCount = 0;
            while (retriesCount < 15)
                try
                {
                    _connection ??= factory.CreateConnection();
                    
                    if (_channel == null)
                    {
                        _channel = _connection.CreateModel();
                        _channel.BasicQos(0, 1, false);
                    } 
                    
                    break;
                }
                catch (BrokerUnreachableException)
                {
                    Task.Delay(1000).Wait();
                    retriesCount++;
                }
        }
    }

    private void AddQueue(string queueName, int? messageLifetime = null)
    {
        Connect();
        _channel.QueueDeclare(queueName, true, false, false, null);
    }

    /// <inheritdoc />
    public async Task Subscribe<T>(string queueName, OnDataReceiveEvent<T>? onReceive)
    {
        if (onReceive == null)
            return;

        // ReSharper disable once AsyncVoidLambda
        await RegisterListener(queueName, async (_, eventArgs) =>
        {
            var channel = GetChannel();
            try
            {
                var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

                var obj = JsonSerializer.Deserialize<T>(message ?? "");

                await onReceive(obj!);
                channel.BasicAck(eventArgs.DeliveryTag, false);
            }
            catch (Exception)
            {
                channel.BasicNack(eventArgs.DeliveryTag, false, false);
            }
        });
    }

    /// <inheritdoc />
    public async Task PushAsync<T>(string queueName, T data)
    {
        await Publish(queueName, data);
    }
}