using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF
{
    public class AppDbContext : IdentityDbContext
    {
        public DbSet<AppRole> AppRoles { get; set; } = default!;
        public DbSet<AppUser> AppUsers { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Comment> Comments { get; set; } = default!;
        public DbSet<Feature> Features { get; set; } = default!;
        public DbSet<FeatureInVoting> FeatureInVotings { get; set; } = default!;
        public DbSet<FeatureStatus> FeatureStatuses { get; set; } = default!;
        public DbSet<Voting> Votings { get; set; } = default!;
        public DbSet<VotingStatus> VotingStatuses { get; set; } = default!;
        public DbSet<UserInVoting> UserInVotings { get; set; } = default!;
        public DbSet<UsersFeaturePriority> UsersFeaturePriorities { get; set; } = default!;
        public DbSet<PriorityStatus> PriorityStatuses { get; set; } = default!;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
