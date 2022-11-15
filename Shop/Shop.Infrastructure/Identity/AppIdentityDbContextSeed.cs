using Microsoft.AspNetCore.Identity;
using Shop.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Yarik",
                    Email = "yarik@gmail.com",
                    UserName = "yarik@gmail.com",
                    Address = new Address()
                    {
                        FirstName = "Devon",
                        LastName = "Craw",
                        Street = "10 Street",
                        State = "NY",
                        Zipcode = "12345"
                    }
                };

                await userManager.CreateAsync(user, "Passw0rd!");
            }
        }
    }
}
