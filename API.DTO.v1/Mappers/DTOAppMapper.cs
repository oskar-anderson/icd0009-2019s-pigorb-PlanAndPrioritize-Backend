using AutoMapper;
using BLL.App.DTO;
using ee.itcollege.pigorb.bookswap.BLL.Base.Mappers;

namespace API.DTO.v1.Mappers
{
    public class DTOAppMapper<TLeft, TRight> :  BaseBLLMapper<TLeft, TRight> 
        where TLeft : class, new() 
        where TRight : class, new()
    {
        public DTOAppMapper()
        {
            MapperConfigurationExpression.CreateMap<AppUserBllDto, AppUserApiDto>();
            MapperConfigurationExpression.CreateMap<CategoryBllDto, CategoryApiDto>();
            MapperConfigurationExpression.CreateMap<CategoryEditBllDto, CategoryEditApiDto>();
            MapperConfigurationExpression.CreateMap<CommentBllDto, CommentApiDto>();
            MapperConfigurationExpression.CreateMap<FeatureBllDto, FeatureApiDto>();
            MapperConfigurationExpression.CreateMap<FeatureInVotingBllDto, FeatureInVotingApiDto>();
            MapperConfigurationExpression.CreateMap<UserInVotingBllDto, UserInVotingApiDto>();
            MapperConfigurationExpression.CreateMap<UsersFeaturePriorityBllDto, UsersFeaturePriorityApiDto>();
            MapperConfigurationExpression.CreateMap<VotingBllDto, VotingApiDto>();
            
            MapperConfigurationExpression.CreateMap<AppUserApiDto, AppUserBllDto>();
            MapperConfigurationExpression.CreateMap<CategoryApiDto, CategoryBllDto>();
            MapperConfigurationExpression.CreateMap<CategoryEditApiDto, CategoryEditBllDto>();
            MapperConfigurationExpression.CreateMap<CategoryCreateApiDto, CategoryBllDto>();
            MapperConfigurationExpression.CreateMap<CommentApiDto, CommentBllDto>();
            MapperConfigurationExpression.CreateMap<FeatureApiDto, FeatureBllDto>();
            MapperConfigurationExpression.CreateMap<FeatureInVotingApiDto, FeatureInVotingBllDto>();
            MapperConfigurationExpression.CreateMap<UserInVotingApiDto, UserInVotingBllDto>();
            MapperConfigurationExpression.CreateMap<UsersFeaturePriorityApiDto, UsersFeaturePriorityBllDto>();
            MapperConfigurationExpression.CreateMap<VotingApiDto, VotingBllDto>();
            
            Mapper = new Mapper(new MapperConfiguration(MapperConfigurationExpression));
        }
    }
}