using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using WestoverLaneReserve.Data;
using WestoverLaneReserve.Models;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<CustomerApplicationUser>>();

            // Check if any users exist
            if (!userManager.Users.Any())
            {
                var users = new CustomerApplicationUser[]
                {
                    new CustomerApplicationUser
                    {
                        UserName = "karnold@example.com",
                        Email = "karnold@example.com",
                        FirstName = "Kyisha",
                        LastName = "Arnold"
                    },
                    new CustomerApplicationUser
                    {
                        UserName = "dmartin@example.com",
                        Email = "dmartin@example.com",
                        FirstName = "Betty",
                        LastName = "Martinez"
                    }
                };

                foreach (var user in users)
                {
                    var result = await userManager.CreateAsync(user, "Password123!"); // Use a generic password for seeding, consider a more secure approach for production
                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException("Failed to create default users.");
                    }
                }
            }
        }
    }
}
