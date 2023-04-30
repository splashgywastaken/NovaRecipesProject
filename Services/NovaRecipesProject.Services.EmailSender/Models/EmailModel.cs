namespace NovaRecipesProject.Services.EmailSender.Models;

/// <summary>
/// DTO model used for sending emails
/// </summary>
public class EmailModel
{
    /// <summary>
    /// 
    /// </summary>
    public string Email { get; set; } = null!;
    /// <summary>
    /// 
    /// </summary>
    public string Subject { get; set; } = null!;
    /// <summary>
    /// 
    /// </summary>
    public string Message { get; set; } = null!;
    /// <summary>
    /// 
    /// </summary>
    public string? From { get; set; } 
}
