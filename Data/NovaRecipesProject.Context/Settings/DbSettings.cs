namespace NovaRecipesProject.Context.Settings;

public class DbSettings
{
    public DbType Type { get; private set; }
    public string ConnectionString { get; private set; } = null!;
}
