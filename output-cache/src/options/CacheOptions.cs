namespace output_cache.options;

public class CacheOptions
{
    public const string ConfigurationSection = "RedisOptions";

    public string ConnectionString { get; set; } = string.Empty;
    public string InstanceName { get; set; } = string.Empty;
    public int ConnectRetry { get; set; } = 3;
    public int ConnectTimeout { get; set; } = 5000;
    public int AsyncTimeout { get; set; } = 300;
    public bool AbortOnConnectFail { get; set; } = false;
    public bool AllowAdmin { get; set; } = false;
    
} 