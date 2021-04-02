using AutoMapper;
using DAL.App.DTO;
using ee.itcollege.pigorb.bookswap.BLL.Base.Mappers;

namespace BLL.App.DTO.Mappers
{
    public class BLLAppMapper<TLeft, TRight> :  BaseBLLMapper<TLeft, TRight> 
        where TLeft : class, new() 
        where TRight : class, new()
    {
        public BLLAppMapper()
        {
            MapperConfigurationExpression.CreateMap<AppUserDalDto, AppUserBllDto>();
            MapperConfigurationExpression.CreateMap<CategoryDalDto, CategoryBllDto>();
            MapperConfigurationExpression.CreateMap<CategoryDalDto, CategoryEditBllDto>();
            MapperConfigurationExpression.CreateMap<CommentDalDto, CommentBllDto>();
            MapperConfigurationExpression.CreateMap<FeatureDalDto, FeatureBllDto>();
            MapperConfigurationExpression.CreateMap<FeatureInVotingDalDto, FeatureInVotingBllDto>();
            MapperConfigurationExpression.CreateMap<UserInVotingDalDto, UserInVotingBllDto>();
            MapperConfigurationExpression.CreateMap<UsersFeaturePriorityDalDto, UsersFeaturePriorityBllDto>();
            MapperConfigurationExpression.CreateMap<VotingDalDto, VotingBllDto>();
            
            MapperConfigurationExpression.CreateMap<AppUserBllDto, AppUserDalDto>();
            MapperConfigurationExpression.CreateMap<CategoryBllDto, CategoryDalDto>();
            MapperConfigurationExpression.CreateMap<CategoryEditBllDto, CategoryDalDto>();
            MapperConfigurationExpression.CreateMap<CommentBllDto, CommentDalDto>();
            MapperConfigurationExpression.CreateMap<FeatureBllDto, FeatureDalDto>();
            MapperConfigurationExpression.CreateMap<FeatureInVotingBllDto, FeatureInVotingDalDto>();
            MapperConfigurationExpression.CreateMap<UserInVotingBllDto, UserInVotingDalDto>();
            MapperConfigurationExpression.CreateMap<UsersFeaturePriorityBllDto, UsersFeaturePriorityDalDto>();
            MapperConfigurationExpression.CreateMap<VotingBllDto, VotingDalDto>();
            
            Mapper = new Mapper(new MapperConfiguration(MapperConfigurationExpression));
        }
    }
}