using System;
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
    public class CommentRepository : EFBaseRepository<AppDbContext, Comment, CommentDalDto>, ICommentRepository
    {
        private readonly DALCommentMapper _mapper = new DALCommentMapper();
        
        public CommentRepository(AppDbContext dbContext) : base(dbContext, new DALCommentMapper())
        {
        }

        public async Task<IEnumerable<CommentDalDto>> GetCommentsForFeature(Guid featureId)
        {
            var comments = RepoDbContext.Comments
                .Where(c => c.FeatureId == featureId)
                .Include(c => c.AppUser)
                .OrderBy(c => c.TimeCreated)
                .Select(dbEntity => _mapper.Map(dbEntity))
                .AsNoTracking();
            return await comments.ToListAsync();
        }
    }
}
