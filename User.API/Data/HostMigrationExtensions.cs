using System;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace User.API.Data
{
    public static class HostMigrationExtensions
    {
        public static IHost MigrateDbContext<TContext>(
            this IHost host,
            Action<TContext, IServiceProvider> seeder
            ) where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var logger = serviceProvider.GetService<ILogger<TContext>>();
            var context = serviceProvider.GetService<TContext>();

            try
            {
                context.Database.Migrate();
                ActionRetry.RetryDoWork<TContext, IServiceProvider>(
                    (context, serviceProvider) => seeder(context, serviceProvider), context, serviceProvider, 10
                );
                logger.LogInformation($"-------------Initial DbContext {typeof(TContext)} Success.-------------------------");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"---------------Initial DbContext {typeof(TContext)} Fail.----------------------------");
            }
            return host;
        }
    }
}