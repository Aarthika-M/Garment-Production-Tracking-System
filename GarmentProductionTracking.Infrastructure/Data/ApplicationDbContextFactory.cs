using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            string? connectionString = null;
            var basePath = Directory.GetCurrentDirectory();
            var searchPaths = new[]
            {
                basePath,
                Path.Combine(basePath, "..", "GarmentProductionTracking.WebUI"),
                Path.Combine(basePath, "GarmentProductionTracking.WebUI")
            };

            foreach (var path in searchPaths)
            {
                var appSettingsPath = Path.Combine(path, "appsettings.json");
                if (File.Exists(appSettingsPath))
                {
                    try
                    {
                        var json = File.ReadAllText(appSettingsPath);
                        var jObject = JObject.Parse(json);
                        connectionString = jObject["ConnectionStrings"]?["DefaultConnection"]?.ToString();
                        if (!string.IsNullOrEmpty(connectionString))
                        {
                            break;
                        }
                    }
                    catch
                    {
                        // Ignore and try next path
                    }
                }
            }

            if (string.IsNullOrEmpty(connectionString))
            {
                // Fallback connection string matching appsettings.json
                connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=aarthika;SearchPath=aa;";
            }

            optionsBuilder.UseNpgsql(
                connectionString,
                npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "aa")
            );

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
