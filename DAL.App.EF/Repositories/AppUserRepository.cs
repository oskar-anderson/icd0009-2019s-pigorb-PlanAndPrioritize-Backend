using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.DTO.Mappers;
using Domain;
using ee.itcollege.pigorb.bookswap.DAL.Base.EF.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DAL.App.EF.Repositories
{
    public class AppUserRepository : EFBaseRepository<AppDbContext, AppUser, AppUserDalDto>, IAppUserRepository
    {
        private readonly DALAppUserMapper _mapper = new DALAppUserMapper();
        //private readonly DALAppRoleMapper _roleMapper = new DALAppRoleMapper();
        private readonly UserManager<AppUser> _userManager;
        //private readonly RoleManager<AppRole> _roleManager;

        public AppUserRepository(AppDbContext dbContext) : base(dbContext, new DALAppUserMapper())
        {
            _userManager = dbContext.GetService<UserManager<AppUser>>();
           // _roleManager = dbContext.GetService<RoleManager<AppRole>>();
        }
        
      }
}
