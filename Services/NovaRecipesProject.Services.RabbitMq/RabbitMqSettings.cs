namespace NovaRecipesProject.Services.RabbitMq;

/// <summary>
/// Settings dto model to get data from appsettings.json file
/// </summary>
public class RabbitMqSettings
{
    /// <summary>
    /// URI to connect to 
    /// </summary>
    public string Uri { get; set; } = null!;
    /// <summary>
    /// User's username in rabbitmq
    /// </summary>
    public string UserName { get; set; } = null!;
    /// <summary>
    /// User's password in rabbitmq
    /// </summary>
    public string Password { get; set; } = null!;
}