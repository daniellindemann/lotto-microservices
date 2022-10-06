public class RedisConfig
{
    public static readonly string ConnectionStringName = "Redis";

    public bool Enabled { get; set; }
    public string? Instance { get; set; }
    public string? DaprStoreId { get; set; }
}