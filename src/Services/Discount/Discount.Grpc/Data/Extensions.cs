using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public static class Extensions
    {
        public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            using var dbContext = serviceScope.ServiceProvider.GetRequiredService<DiscountContext>();
            dbContext.Database.MigrateAsync();
            
            return app;
        }
    }
}
