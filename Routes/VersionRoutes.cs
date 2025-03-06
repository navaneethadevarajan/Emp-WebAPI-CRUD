public static class VersionRoutes
{
    public static void RegisterVersionRoutes(this WebApplication app)
    {
        app.MapGet("/api/version", () =>
        {
            var version = "25.03.05"; // You can fetch this from appsettings.json
            return Results.Ok(new { version });
        })
        .WithName("GetVersion")
        .WithOpenApi();
    }
}
