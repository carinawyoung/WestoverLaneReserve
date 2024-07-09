using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using WestoverLaneReserve.Data;
using WestoverLaneReserve.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
        {
            // Look for any customers
            if (context.Customers.Any())
            {
                return;   // database has been seeded
            }

            context.Customers.AddRange(
                new Customer
                {
                    FirstName = "Kyisha",
                    LastName = "Arnold",
                    Email = "karnold@example.come",
                    Password = "Password123!"
                },
                new Customer
                {
                    FirstName = "Betty",
                    LastName = "Martinez",
                    Email = "dmartin@example.com",
                    Password = "Password321!"
                }
            );

            context.SaveChanges();
        }
    }
}