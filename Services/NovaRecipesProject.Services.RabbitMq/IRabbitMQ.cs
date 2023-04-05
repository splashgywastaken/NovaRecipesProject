namespace NovaRecipesProject.Services.RabbitMq;

using System.Threading.Tasks;

/// <summary>
/// Delegate to use when new data is received 
/// </summary>
/// <typeparam name="T"></typeparam>
public delegate Task OnDataReceiveEvent<in T>(T data);

/// <summary>
/// Rabbit mq interface 
/// </summary>
public interface IRabbitMq
{
    /// <summary>
    /// Method that subscribes to certaine message in queue
    /// </summary>
    /// <param name="queueName"></param>
    /// <param name="onReceive"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task Subscribe<T>(string queueName, OnDataReceiveEvent<T>? onReceive);
    /// <summary>
    /// Method that pushes data to certaine queue
    /// </summary>
    /// <param name="queueName"></param>
    /// <param name="data"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task PushAsync<T>(string queueName, T data);
}