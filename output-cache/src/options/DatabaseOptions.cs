namespace output_cache.options;

public class DatabaseOptions
{
    public const string ConfigurationSection = "DatabaseOptions";

    public string ConnectionString { get; set; } = string.Empty;
    public int MaxRetryCount { get; set; } = 3;
    public int CommandTimeout { get; set; } = 30;
    public bool EnableDetailedErrors { get; set; } = false;
    public bool EnableSensitiveDataLogging { get; set; } = false;
    public int MaxRetryDelaySeconds { get; set; }
} 