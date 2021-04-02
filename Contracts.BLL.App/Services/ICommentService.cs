using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.DAL.App.Repositories;

namespace Contracts.BLL.App.Services
{
    public interface ICommentService : ICommentRepository<CommentBllDto>
    {
        Task<IEnumerable<CommentBllDto>> GetCommentsForFeature(Guid featureId);
    }
}