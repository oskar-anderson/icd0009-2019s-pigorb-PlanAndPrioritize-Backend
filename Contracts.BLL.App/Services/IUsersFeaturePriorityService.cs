using System;
using System.Collections.Generic;
using API.DTO.v1;
using BLL.App.DTO;
using Contracts.DAL.App.Repositories;

namespace Contracts.BLL.App.Services
{
    public interface IUsersFeaturePriorityService : IUsersFeaturePriorityRepository<UsersFeaturePriorityBllDto>
    {
        void AddUserPriorities(Dictionary<Guid, UsersFeaturePriorityCreateApiDto> featureInVotings, Guid userId);
        void UpdateUserPriorities(Dictionary<Guid, UsersFeaturePriorityCreateApiDto> featureInVotings, Guid userId);
    }
}