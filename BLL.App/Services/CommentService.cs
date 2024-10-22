using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using BLL.App.DTO.Mappers;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using ee.itcollege.pigorb.bookswap.BLL.Base.Services;

namespace BLL.App.Services
{
    public class CommentService : BaseEntityService<ICommentRepository, IAppUnitOfWork, CommentDalDto, CommentBllDto>, ICommentService
    {
        private readonly BLLCommentMapper _mapper = new BLLCommentMapper();
        
        public CommentService(IAppUnitOfWork unitOfWork)
            : base(unitOfWork, new BLLCommentMapper(), unitOfWork.Comments)
        {
        }

        public async Task<IEnumerable<CommentBllDto>> GetCommentsForFeature(Guid featureId)
        {
            return (await ServiceRepository.GetCommentsForFeature(featureId)).Select(dalEntity => _mapper.Map(dalEntity));
        }
    }
}