namespace NovaRecipesProject.Context.Settings;

/// <summary>
/// DbSettings class used in setup for application
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class DbSettings
{
    /// <summary>
    /// DbType to decide which Dn to use
    /// </summary>
    public DbType Type { get; private set; }
    /// <summary>
    /// Connection string for DB to connect to 
    /// </summary>
    public string ConnectionString { get; private set; } = null!;
}
