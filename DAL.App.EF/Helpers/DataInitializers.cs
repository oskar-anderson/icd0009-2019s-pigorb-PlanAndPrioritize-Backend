using System;
using System.Linq;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF.Helpers
{
    public class DataInitializers
    {
        public static void MigrateDatabase(AppDbContext context)
        {
            context.Database.Migrate();
        }
        public static bool DeleteDatabase(AppDbContext context)
        {
            return context.Database.EnsureDeleted();
        }

        public static void SeedIdentity(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            var roleNames = new string[] {"User", "Admin"};
            foreach (var roleName in roleNames)
            {
                var role = roleManager.FindByNameAsync(roleName).Result;
                if (role == null)
                {
                    role = new AppRole {Name = roleName};
                    var result = roleManager.CreateAsync(role).Result;
                    if (!result.Succeeded)
                    {
                        throw new ApplicationException("Role creation failed!");
                    }
                }
            }
            
            var userName = "piret@mail.ee";
            var passWord = "Test_1";
            var firstName = "Piret";
            var lastName = "Gorban";

            var user = userManager.FindByNameAsync(userName).Result;
            if (user == null)
            {
                user = new AppUser {Email = userName, UserName = userName, FirstName = firstName, LastName = lastName};

                var result = userManager.CreateAsync(user, passWord).Result;
                if (!result.Succeeded)
                {
                    throw new ApplicationException("User creation failed!");
                }
            }
            
            var roleResult = userManager.AddToRoleAsync(user, "Admin").Result;
            roleResult = userManager.AddToRoleAsync(user, "User").Result;
        }
        
        public static void SeedData(AppDbContext context)
        {
            var categories = new Category[]
            {
                new Category
                {
                    Title = "Maintenance", Description = "Development work necessary for systems maintenance"
                },
                new Category
                {
                    Title = "Web shop development"
                }
            };

            foreach (var category in categories)
            {
                if (!context.Categories.Any(g => g.Id == category.Id))
                {
                    context.Categories.Add(category);
                }
            }

            context.SaveChanges();
        }
        

    }
}
