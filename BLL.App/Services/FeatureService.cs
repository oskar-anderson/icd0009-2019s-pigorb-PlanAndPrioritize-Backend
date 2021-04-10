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

        public async Task<IEnumerable<FeatureBllDto>> GetAllWithoutCollections(string? search)
        {
            return (await ServiceRepository.GetAllWithoutCollections(search)).Select(dalEntity => _mapper.Map(dalEntity));
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
            return _mapper.Map(await ServiceRepository.FirstOrDefault(id));
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