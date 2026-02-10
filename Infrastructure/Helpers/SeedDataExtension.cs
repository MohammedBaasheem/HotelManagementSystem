using Infrastructure.Data;
using Infrastructure.Seeds;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Helpers
{
    public static class SeedDataExtension
    {
        public static async Task SeedAsync(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<Context>();
                await ContextSeed.SeedAsync(context);
            }
        }
    }
}
