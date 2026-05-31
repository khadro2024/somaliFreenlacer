namespace SomaliFreelanceMarketplace.Helpers;

/// <summary>
/// Converts Neon/Railway DATABASE_URL (postgresql://...) to Npgsql connection string.
/// </summary>
public static class DatabaseUrlHelper
{
    public static string? ResolveConnectionString(IConfiguration configuration)
    {
        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
        if (!string.IsNullOrWhiteSpace(databaseUrl))
            return ParsePostgresUrl(databaseUrl);

        return configuration.GetConnectionString("DefaultConnection");
    }

    public static string ParsePostgresUrl(string databaseUrl)
    {
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':', 2);
        var username = Uri.UnescapeDataString(userInfo[0]);
        var password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : string.Empty;
        var host = uri.Host;
        var dbPort = uri.Port > 0 ? uri.Port : 5432;
        var database = uri.AbsolutePath.TrimStart('/');

        var sslMode = "Require";
        if (!string.IsNullOrEmpty(uri.Query))
        {
            foreach (var part in uri.Query.TrimStart('?').Split('&'))
            {
                var kv = part.Split('=', 2);
                if (kv.Length == 2 && kv[0].Equals("sslmode", StringComparison.OrdinalIgnoreCase))
                    sslMode = char.ToUpperInvariant(kv[1][0]) + kv[1][1..];
            }
        }

        return $"Host={host};Port={dbPort};Database={database};Username={username};Password={password};SSL Mode={sslMode};Trust Server Certificate=true";
    }
}
