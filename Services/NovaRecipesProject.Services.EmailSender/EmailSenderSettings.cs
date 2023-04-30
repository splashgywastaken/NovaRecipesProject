namespace NovaRecipesProject.Services.MailSender;

/// <summary>
/// Class used to hold settings (aka parameters) for mail sender
/// </summary>
public class EmailSenderSettings
{
    /// <summary>
    /// smtp server address 
    /// </summary>
    /// <returns></returns>
    public string SmtpServer { get; set; } = null!;
    /// <summary>
    /// Port for smtp server
    /// </summary>
    public int SmtpPort { get; set; }
    /// <summary>
    /// Check for using/not using ssl
    /// </summary>
    public bool UseSsl { get; set; }
    /// <summary>
    /// User's username
    /// </summary>
    public string Username { get; set; } = null!;
    /// <summary>
    /// Sender's address, this will be used as default sender if other is not used
    /// </summary>
    public string SenderAddress { get; set; } = null!;
    /// <summary>
    /// Used for mailkits message system 
    /// </summary>
    public string SenderName { get; set; } = null!;
    /// <summary>
    /// User's password
    /// </summary>
    public string Password { get; set; } = null!;
}