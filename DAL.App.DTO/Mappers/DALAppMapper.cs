using AutoMapper;
using Domain;
using ee.itcollege.pigorb.bookswap.DAL.Base.EF.Mappers;

namespace DAL.App.DTO.Mappers
{
    public class DALAppMapper<TLeft, TRight> :  BaseDALMapper<TLeft, TRight> 
        where TLeft : class, new() 
        where TRight : class, new()
    {
        public DALAppMapper()
        {
            MapperConfigurationExpression.CreateMap<AppUser, AppUserDalDto>();
            MapperConfigurationExpression.CreateMap<Category, CategoryDalDto>();
            MapperConfigurationExpression.CreateMap<Comment, CommentDalDto>();
            MapperConfigurationExpression.CreateMap<Feature, FeatureDalDto>();
            MapperConfigurationExpression.CreateMap<FeatureInVoting, FeatureInVotingDalDto>();
            MapperConfigurationExpression.CreateMap<FeatureStatus, FeatureStatusDalDto>();
            MapperConfigurationExpression.CreateMap<PriorityStatus, PriorityStatusDalDto>();
            MapperConfigurationExpression.CreateMap<UserInVoting, UserInVotingDalDto>();
            MapperConfigurationExpression.CreateMap<UsersFeaturePriority, UsersFeaturePriorityDalDto>();
            MapperConfigurationExpression.CreateMap<Voting, VotingDalDto>();
            MapperConfigurationExpression.CreateMap<VotingStatus, VotingStatusDalDto>();
            
            MapperConfigurationExpression.CreateMap<AppUserDalDto, AppUser>();
            MapperConfigurationExpression.CreateMap<CategoryDalDto, Category>();
            MapperConfigurationExpression.CreateMap<CommentDalDto, Comment>();
            MapperConfigurationExpression.CreateMap<FeatureDalDto, Feature>();
            MapperConfigurationExpression.CreateMap<FeatureInVotingDalDto, FeatureInVoting>();
            MapperConfigurationExpression.CreateMap<FeatureStatusDalDto, FeatureStatus>();
            MapperConfigurationExpression.CreateMap<PriorityStatusDalDto, PriorityStatus>();
            MapperConfigurationExpression.CreateMap<UserInVotingDalDto, UserInVoting>();
            MapperConfigurationExpression.CreateMap<UsersFeaturePriorityDalDto, UsersFeaturePriority>();
            MapperConfigurationExpression.CreateMap<VotingDalDto, Voting>();
            MapperConfigurationExpression.CreateMap<VotingStatusDalDto, VotingStatus>();

            Mapper = new Mapper(new MapperConfiguration(MapperConfigurationExpression));
        }        
    }
    
}