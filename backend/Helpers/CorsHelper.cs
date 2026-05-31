namespace SomaliFreelanceMarketplace.Helpers;

public static class CorsHelper
{
    public static string[] ResolveAllowedOrigins(IConfiguration configuration)
    {
        var origins = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var origin in configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [])
        {
            if (!string.IsNullOrWhiteSpace(origin))
                origins.Add(NormalizeOrigin(origin));
        }

        var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL");
        if (!string.IsNullOrWhiteSpace(frontendUrl))
        {
            foreach (var origin in frontendUrl.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                origins.Add(NormalizeOrigin(origin));
        }

        var allowedOrigins = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS");
        if (!string.IsNullOrWhiteSpace(allowedOrigins))
        {
            foreach (var origin in allowedOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                origins.Add(NormalizeOrigin(origin));
        }

        if (origins.Count == 0)
            origins.Add("http://localhost:5173");

        return origins.ToArray();
    }

    private static string NormalizeOrigin(string origin) =>
        origin.Trim().TrimEnd('/');
}
