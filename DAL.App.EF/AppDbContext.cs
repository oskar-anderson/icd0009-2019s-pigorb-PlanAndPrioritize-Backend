using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>, IBaseEntityTracker 
    {
        private readonly IUserNameProvider _userNameProvider;
        
        public DbSet<AppRole> AppRoles { get; set; } = default!;
        public DbSet<AppUser> AppUsers { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Comment> Comments { get; set; } = default!;
        public DbSet<Feature> Features { get; set; } = default!;
        public DbSet<FeatureInVoting> FeatureInVotings { get; set; } = default!;
        public DbSet<Voting> Votings { get; set; } = default!;
        public DbSet<UserInVoting> UserInVotings { get; set; } = default!;
        public DbSet<UsersFeaturePriority> UsersFeaturePriorities { get; set; } = default!;
        
        private readonly Dictionary<IDomainBaseEntity<Guid>, IDomainBaseEntity<Guid>> _entityTracker
            = new Dictionary<IDomainBaseEntity<Guid>, IDomainBaseEntity<Guid>>();

        public AppDbContext(DbContextOptions<AppDbContext> options, IUserNameProvider userNameProvider)
            : base(options)
        {
            _userNameProvider = userNameProvider;
        }
        
        public void AddToEntityTracker(IDomainBaseEntity<Guid> internalEntity, IDomainBaseEntity<Guid> externalEntity)
        {
            _entityTracker.Add(internalEntity, externalEntity);
        }
        
        private void UpdateTrackedEntities()
        {
            foreach (var (key, value) in _entityTracker)
            {
                value.Id = key.Id;
            }
        }

        public override int SaveChanges()
        {
            var result = base.SaveChanges();
            UpdateTrackedEntities();
            return result;

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = base.SaveChangesAsync(cancellationToken);
            UpdateTrackedEntities();
            return result;
        }
        
        
    }
}
