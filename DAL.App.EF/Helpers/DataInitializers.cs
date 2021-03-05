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

            var featureStatuses = new FeatureStatus[]
            {
                new FeatureStatus
                {
                    Name = "To do", Description = "Tasks created get this status initially"
                },
                new FeatureStatus
                {
                    Name = "In progress"
                },
                new FeatureStatus
                {
                    Name = "In review"
                },
                new FeatureStatus
                {
                    Name = "Done"
                }
            };
            
            var votingStatuses = new VotingStatus[]
            {
                new VotingStatus
                {
                    Name = "Not open yet", Description = "Priority voting has been created but no dates assigned or open dates are in the future"
                },
                new VotingStatus
                {
                    Name = "Open", Description = "Priorities can be added"
                },
                new VotingStatus
                {
                    Name = "Closed", Description = "Priority voting dates are in the past"
                }
            };
            
            /***
            var features = new Feature[]
            {
                new Feature
                {
                    Title = "Add new button to web shop", 
                    Description = "New button is necessary for adding multiple items to cart", 
                    TimeCreated = DateTime.Now, 
                    LastEdited = DateTime.Now
                },
                new Feature
                {
                    
                },
                new Feature
                {
                    
                }
            };
            */

            foreach (var category in categories)
            {
                if (!context.Categories.Any(g => g.Id == category.Id))
                {
                    context.Categories.Add(category);
                }
            }
            
            foreach (var featureStatus in featureStatuses)
            {
                if (!context.FeatureStatuses.Any(g => g.Id == featureStatus.Id))
                {
                    context.FeatureStatuses.Add(featureStatus);
                }
            }
            
            foreach (var votingStatus in votingStatuses)
            {
                if (!context.VotingStatuses.Any(g => g.Id == votingStatus.Id))
                {
                    context.VotingStatuses.Add(votingStatus);
                }
            }
            
            context.SaveChanges();
        }
        

    }
}
