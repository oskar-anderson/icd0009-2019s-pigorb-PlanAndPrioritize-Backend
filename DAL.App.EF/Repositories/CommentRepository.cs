using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.DTO.Mappers;
using Domain;
using ee.itcollege.pigorb.bookswap.DAL.Base.EF.Repositories;

namespace DAL.App.EF.Repositories
{
    public class CommentRepository : EFBaseRepository<AppDbContext, Comment, CommentDalDto>, ICommentRepository
    {
        private readonly DALCommentMapper _mapper = new DALCommentMapper();
        
        public CommentRepository(AppDbContext dbContext) : base(dbContext, new DALCommentMapper())
        {
        }
        
      }
}
