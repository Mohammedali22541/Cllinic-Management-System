using ClinicApp.Data.Context;
using ClinicApp.Data.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Clinic_App.ExtensionMethods
{
    public static class WebApplicationRegister
    {
        public static async Task<WebApplication> SeedDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dataIntializer = scope.ServiceProvider.GetRequiredService<IDataInitializer>();
            await dataIntializer.InitializeIdentityDataAsync();

            return app;
        }

        public static async Task<WebApplication> ApplyPendingMigrationsAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ClinicManagementDbContext>();
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                await dbContext.Database.MigrateAsync();
            }
            return app;
        }
    }
}