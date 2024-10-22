﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.DTO.Mappers;
using Domain;
using ee.itcollege.pigorb.bookswap.DAL.Base.EF.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF.Repositories
{
    public class CategoryRepository : EFBaseRepository<AppDbContext, Category, CategoryDalDto>, ICategoryRepository
    {
        private readonly DALCategoryMapper _mapper = new DALCategoryMapper();
        
        public CategoryRepository(AppDbContext dbContext) : base(dbContext, new DALCategoryMapper())
        {
        }
        
        public async Task<IEnumerable<CategoryDalDto>> GetAll()
        {
            var categories = RepoDbSet
                .Include(c => c.Features)
                .OrderBy(c => c.Title)
                .Select(dbEntity => _mapper.Map(dbEntity))
                .AsNoTracking();
            return await categories.ToListAsync();
        }

        public async Task<IEnumerable<CategoryDalDto>> GetAllPlain()
        {
            var categories = RepoDbSet
                .OrderBy(c => c.Title)
                .Select(dbEntity => _mapper.Map(dbEntity))
                .AsNoTracking();
            return await categories.ToListAsync();
        }

        public async Task<bool> Exists(Guid id)
        {
            return await RepoDbSet.AnyAsync(a => a.Id == id);
        }

        public async Task<CategoryDalDto> FirstOrDefault(Guid id)
        {
            var query = RepoDbSet
                .Where(a => a.Id == id)
                .AsQueryable();
            return Mapper.Map(await query.AsNoTracking().FirstOrDefaultAsync());
        }

        public async Task Delete(Guid id)
        {
            var query = RepoDbSet.Where(a => a.Id == id).AsQueryable();
            
            var category = await query.AsNoTracking().FirstOrDefaultAsync();
            base.Remove(category.Id);
        }

        public CategoryDalDto Edit(CategoryDalDto dalEntity)
        {
            var entity = Mapper.Map(dalEntity);
            
            var trackedEntity = RepoDbSet.Update(entity).Entity;
            var result = Mapper.Map(trackedEntity);
            return result;
        }
    }
}
