using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO.v1;
using BLL.App.DTO;
using BLL.App.DTO.Mappers;
using Classifiers;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using ee.itcollege.pigorb.bookswap.BLL.Base.Services;

namespace BLL.App.Services
{
    public class FeatureService : BaseEntityService<IFeatureRepository, IAppUnitOfWork, FeatureDalDto, FeatureBllDto>, IFeatureService
    {
        private readonly BLLFeatureMapper _mapper = new ();
        
        public FeatureService(IAppUnitOfWork unitOfWork)
            : base(unitOfWork, new BLLFeatureMapper(), unitOfWork.Features)
        {
        }

        public async Task<IEnumerable<FeatureBllDto>> GetAll()
        {
            return (await ServiceRepository.GetAll()).Select(dalEntity => _mapper.Map(dalEntity));
        }

        public IEnumerable<FeatureBllDto> GetAllWithVotings(string? search)
        {
            var features = ServiceRepository.GetAllWithVotings(search).Select(dalEntity => _mapper.Map(dalEntity)).ToList();
            foreach (var feature in features)
            {
                if (feature.FeatureInVotings == null) continue;
                var latestFeatureInVoting = GetLatestFeatureInVoting(feature.FeatureInVotings);
                if (latestFeatureInVoting?.Voting == null) continue;
                
                if (latestFeatureInVoting.Voting.EndTime >= DateTime.Now)
                {
                    feature.IsInOpenVoting = true;
                }
            }
        
            return features;
        }

        public async Task<IEnumerable<FeatureBllDto>> GetFeaturesForGraph()
        {
            return (await ServiceRepository.GetFeaturesForGraph()).Select(dalEntity => _mapper.Map(dalEntity));
        }

        public async Task<IEnumerable<FeatureBllDto>> GetToDoFeatures()
        {
            return (await ServiceRepository.GetToDoFeatures()).Select(dalEntity => _mapper.Map(dalEntity));
        }

        public async Task<bool> Exists(Guid id)
        {
            return await ServiceRepository.Exists(id);
        }

        public async Task<FeatureBllDto> FirstOrDefault(Guid id)
        {
            var feature = _mapper.Map(await ServiceRepository.FirstOrDefault(id));
            if (feature.FeatureInVotings != null)
            {
                var latestFeatureInVoting = GetLatestFeatureInVoting(feature.FeatureInVotings);
                if (latestFeatureInVoting?.Voting != null && latestFeatureInVoting.Voting.EndTime >= DateTime.Now)
                {
                    feature.IsInOpenVoting = true;
                }
            }
            return feature;
        }

        public async Task<FeatureBllDto> GetFeaturePlain(Guid id)
        {
            return _mapper.Map(await ServiceRepository.GetFeaturePlain(id));
        }

        public async Task Delete(Guid id)
        {
            await ServiceRepository.Delete(id);
        }

        public FeatureBllDto Edit(FeatureBllDto entity)
        {
            return _mapper.Map(ServiceRepository.Edit(_mapper.Map(entity)));
        }

        public async Task<IEnumerable<FeatureBllDto>> GetFeaturesForVoting(Guid votingId)
        {
            var features = (await ServiceRepository.GetAll()).Select(dalEntity => _mapper.MapFeature(dalEntity));
            return features.ToList().FindAll(f => IsInVoting(f, votingId));
        }

        public async Task<IEnumerable<FeatureBllDto>> GetToDoFeaturesNotInVoting(Guid votingId)
        {
            var features = (await ServiceRepository.GetAll()).Select(dalEntity => _mapper.MapFeature(dalEntity));
            return features.ToList().FindAll(f => f.FeatureStatus == FeatureStatus.NotStarted && !IsInVoting(f, votingId));
        }

        public async Task CalculateSizeAndPriority(ICollection<Guid> featureIds)
        {
            foreach (var id in featureIds)
            {
                var feature = await ServiceRepository.FirstOrDefault(id);
                var featureInVotings = feature.FeatureInVotings;
                if (featureInVotings == null || featureInVotings.Count == 0) continue;
                var latestFeatureInVoting = GetLatestFeatureInVoting(featureInVotings);

                var changedFeature = new FeatureDalDto
                {
                    Id = feature.Id,
                    Title = feature.Title,
                    Size = GetSize(latestFeatureInVoting),
                    PriorityValue = GetPriority(latestFeatureInVoting),
                    Description = feature.Description,
                    StartTime = feature.StartTime,
                    EndTime = feature.EndTime,
                    Duration = feature.Duration,
                    CategoryId = feature.CategoryId,
                    FeatureStatus = feature.FeatureStatus,
                    AppUserId = feature.AppUserId,
                    TimeCreated = feature.TimeCreated,
                    CreatedById = feature.CreatedById,
                    LastEdited = feature.LastEdited,
                    ChangeLog = feature.ChangeLog
                };
                ServiceRepository.Update(changedFeature);
            }
        }

        public async Task UpdatePriorityForAllFeatures()
        {
            var features = await ServiceRepository.GetAll();
            var featureIds = features.Select(f => f.Id).ToList();
            await CalculateSizeAndPriority(featureIds);
        }

        public async Task UpdatePriorityForFeature(Guid id)
        {
            var feature = await ServiceRepository.FirstOrDefault(id);
            await CalculateSizeAndPriority(new List<Guid>{ id });
        }

        public async Task<IEnumerable<FeatureWithUsersPriorityBllDto>> GetFeaturesWithUsersPriorities(
            IEnumerable<UsersFeaturePriorityBllDto> userPriorities, Guid votingId)
        {
            var userPrioritiesList = userPriorities.ToList();
            var votedFeaturesWithPriorities = userPrioritiesList.Select(priority => new FeatureWithUsersPriorityBllDto
                {
                    Id = priority.FeatureInVoting!.FeatureId,
                    VotingId = priority.FeatureInVoting.VotingId,
                    Title = priority.FeatureInVoting.Feature!.Title,
                    Description = priority.FeatureInVoting.Feature.Description,
                    CategoryName = priority.FeatureInVoting.Feature.Category!.Title,
                    TaskSize = priority.Size,
                    BusinessValue = priority.BusinessValue,
                    TimeCriticality = priority.TimeCriticality,
                    RiskOrOpportunity = priority.RiskOrOpportunity
                })
                .ToList();

            var votedFeaturesIds = userPrioritiesList.Select(u => u.FeatureInVoting!.FeatureId);
            var allFeaturesForVoting = await GetFeaturesForVoting(votingId);
            var featuresNotVoted = allFeaturesForVoting.Where(f => !votedFeaturesIds.Contains(f.Id));
            
            var featuresWithoutPriorities = featuresNotVoted.Select(feature => new FeatureWithUsersPriorityBllDto
                {
                    Id = feature.Id,
                    VotingId = votingId,
                    Title = feature.Title,
                    Description = feature.Description,
                    CategoryName = feature.Category!.Title
                })
                .ToList();
            
            votedFeaturesWithPriorities.AddRange(featuresWithoutPriorities);
            return votedFeaturesWithPriorities;
        }

        private FeatureInVotingDalDto? GetLatestFeatureInVoting(ICollection<FeatureInVotingDalDto> featureInVotings)
        {
            return featureInVotings.OrderByDescending(f => f.Voting!.EndTime).FirstOrDefault();
        }
        
        private FeatureInVotingBllDto? GetLatestFeatureInVoting(ICollection<FeatureInVotingBllDto> featureInVotings)
        {
            return featureInVotings.OrderByDescending(f => f.Voting!.EndTime).FirstOrDefault();
        }

        private int GetSize(FeatureInVotingDalDto? latestFeatureInVoting)
        {
            return latestFeatureInVoting != null ? decimal.ToInt32(latestFeatureInVoting.AverageSize) : 0;
        }
        
        private decimal GetPriority(FeatureInVotingDalDto? latestFeatureInVoting)
        {
            return latestFeatureInVoting?.AveragePriorityValue ?? 0;
        }

        private bool IsInVoting(FeatureBllDto feature, Guid votingId)
        {
            return feature.FeatureInVotings != null && feature.FeatureInVotings.ToList().Any(f => f.VotingId == votingId);
        }

        public FeatureBllDto AddWithMetaData(FeatureBllDto entity, Guid userId)
        {
            entity.CreatedById = userId;
            entity.FeatureStatus = FeatureStatus.NotStarted;
            entity.TimeCreated = DateTime.Now;
            entity.LastEdited = DateTime.Now;
            
            if (entity.StartTime != null && entity.EndTime != null)
            {
                entity.Duration = entity.StartTime == entity.EndTime ? 1 : (entity.EndTime - entity.StartTime).Value.Days;
            }
            
            return _mapper.Map(ServiceRepository.Add(_mapper.Map(entity)));
        }

        public ICollection<FeatureStatusApiDto> GetFeatureStatuses()
        {
            List<FeatureStatusApiDto> states = Enum.GetValues(typeof(FeatureStatus))
                .Cast<FeatureStatus>()
                .ToList()
                .ConvertAll(s => new FeatureStatusApiDto { State = MapFromFeatureStatus(s)});

            return states;
        }
        
        public async Task<FeatureBllDto> GetLatestFeature()
        {
            return _mapper.Map(await ServiceRepository.GetLatestFeature());
        }

        public FeatureBllDto ConstructEditedFeatureWithChangeLog(FeatureBllDto bllFeature, FeatureEditApiDto apiFeature,
            string userName, CategoryBllDto category, AppUserBllDto? assignee)
        {
            FeatureBllDto editedFeature = new FeatureBllDto
            {
                Id = bllFeature.Id,
                Title = bllFeature.Title,
                Size = bllFeature.Size,
                PriorityValue = bllFeature.PriorityValue,
                Description = bllFeature.Description,
                StartTime = bllFeature.StartTime,
                EndTime = bllFeature.EndTime,
                Duration = bllFeature.Duration,
                CategoryId = bllFeature.CategoryId,
                FeatureStatus = bllFeature.FeatureStatus,
                AppUserId = bllFeature.AppUserId,
                AppUser = bllFeature.AppUser,
                TimeCreated = bllFeature.TimeCreated,
                CreatedById = bllFeature.CreatedById,
                CreatedBy = bllFeature.CreatedBy,
                LastEdited = DateTime.Now,
                ChangeLog = bllFeature.ChangeLog,
            };
            
            if (bllFeature.Title != apiFeature.Title)
            {
                editedFeature.ChangeLog += "\\n" + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + " " + userName + " changed title from '" + bllFeature.Title + "' to '" +
                                           apiFeature.Title + "'.";
                editedFeature.Title = apiFeature.Title;
            }
            
            if (bllFeature.Description != apiFeature.Description)
            {
                editedFeature.ChangeLog += "\\n" + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + " " + userName + " changed description from '" + bllFeature.Description + "' to '" +
                                           apiFeature.Description + "'.";
                editedFeature.Description = apiFeature.Description;
            }
            
            if (bllFeature.CategoryId != apiFeature.CategoryId)
            {
                editedFeature.ChangeLog += "\\n" + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + " " + userName + " changed category from '" + bllFeature.Category!.Title + "' to '" +
                                           category.Title + "'.";
                editedFeature.CategoryId = apiFeature.CategoryId;
            }
            
            if (bllFeature.AppUserId != apiFeature.AppUserId)
            {
                editedFeature.ChangeLog += "\\n" + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + " " + userName + " changed assignee from '" + GetName(bllFeature.AppUser) + "' to '" +
                                           GetName(assignee) + "'.";
                editedFeature.AppUserId = apiFeature.AppUserId;
            }
            
            if (MapFromFeatureStatus(bllFeature.FeatureStatus) != apiFeature.FeatureStatus)
            {
                editedFeature.ChangeLog += "\\n" + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + " " + userName + " changed status from '" + MapFromFeatureStatus(bllFeature.FeatureStatus) + "' to '" +
                                           apiFeature.FeatureStatus + "'.";
                editedFeature.FeatureStatus = MapToFeatureStatus(apiFeature.FeatureStatus);
            }

            if (bllFeature.StartTime != apiFeature.StartTime)
            {
                editedFeature.ChangeLog += "\\n" + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + " " + userName + " changed start date from '" + 
                                           FormatDate(bllFeature.StartTime) + "' to '" + FormatDate(apiFeature.StartTime) + "'.";
                editedFeature.StartTime = apiFeature.StartTime;
            }
            
            if (bllFeature.EndTime != apiFeature.EndTime)
            {
                editedFeature.ChangeLog += "\\n" + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + " " + userName + " changed end date from '" + 
                                           FormatDate(bllFeature.EndTime) + "' to '" + FormatDate(apiFeature.EndTime) + "'.";
                editedFeature.EndTime = apiFeature.EndTime;
            }

            if (editedFeature.StartTime != null && editedFeature.EndTime != null)
            {
                editedFeature.Duration = editedFeature.StartTime == editedFeature.EndTime ? 1 : (editedFeature.EndTime - editedFeature.StartTime).Value.Days;
            }
            if (editedFeature.StartTime == null || editedFeature.EndTime == null)
            {
                editedFeature.Duration = 0;
            }
            if (bllFeature.Duration != editedFeature.Duration)
            {
                editedFeature.ChangeLog += "\\n" + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + " " + userName + " changed duration from '" + bllFeature.Duration + "' to '" +
                                           editedFeature.Duration + "' days.";
            }

            return editedFeature;
        }

        private string MapFromFeatureStatus(FeatureStatus status)
        {
            return status switch
            {
                FeatureStatus.NotStarted => "Not started",
                FeatureStatus.InProgress => "In progress",
                FeatureStatus.InReview => "In review",
                FeatureStatus.ToDeploy => "To deploy",
                FeatureStatus.Closed => "Closed",
                _ => "Incorrect feature status value"
            };
        }
        
        private FeatureStatus MapToFeatureStatus(string status)
        {
            return status switch
            {
                "Not started" => FeatureStatus.NotStarted,
                "In progress" => FeatureStatus.InProgress,
                "In review" => FeatureStatus.InReview,
                "To deploy" => FeatureStatus.ToDeploy,
                "Closed" => FeatureStatus.Closed,
                _ => FeatureStatus.NotStarted
            };
        }

        private string GetName(AppUserBllDto? user)
        {
            if (user == null)
            {
                return "";
            }
            return user.FirstName + " " + user.LastName;
        }

        private string FormatDate(DateTime? date)
        {
            return date == null ? "" : date.Value.ToString("dd.MM.yyyy HH:mm");
        }
    }
}