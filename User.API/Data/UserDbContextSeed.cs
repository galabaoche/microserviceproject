using System;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using User.API.Models;
using System.Threading;

namespace User.API.Data
{
    public class UserDbContextSeed
    {
        public async Task SeedAsync(UserContext context, IServiceProvider serviceProvider)
        {
            if (!context.Users.Any())
            {
                context.Users.Add(new AppUser { Name = "lisa" });
                await context.SaveChangesAsync();
            }
        }
    }
}