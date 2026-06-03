using MythosBalance.Models;
using MythosBalance.Repositories;
using MythosBalance.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MythosBalance.Services
{
    public interface IActivityService
    {
        Task<ActivityIndexViewModel> GetIndexViewModelAsync(string userId, int? domainId, DateTime? start, DateTime? end);
        Task<ActivityCreateViewModel> GetCreateViewModelAsync();
        Task<ActivityEditViewModel?> GetEditViewModelAsync(int id, string userId);
        Task<bool> CreateAsync(ActivityCreateViewModel model, string userId);
        Task<bool> UpdateAsync(ActivityEditViewModel model, string userId);
        Task<bool> DeleteAsync(int id, string userId);
    }

    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepo;
        private readonly ILifeDomainRepository _domainRepo;

        public ActivityService(IActivityRepository activityRepo, ILifeDomainRepository domainRepo)
        {
            _activityRepo = activityRepo;
            _domainRepo = domainRepo;
        }

        public async Task<ActivityIndexViewModel> GetIndexViewModelAsync(string userId, int? domainId, DateTime? start, DateTime? end)
        {
            var activities = await _activityRepo.GetByUserIdFilteredAsync(userId, domainId, start, end);
            var domains = await _domainRepo.GetAllAsync();
            string? filterName = null;
            if (domainId.HasValue)
                filterName = domains.FirstOrDefault(d => d.Id == domainId)?.TurkishName;

            return new ActivityIndexViewModel
            {
                Activities = activities,
                LifeDomains = domains,
                FilterDomainId = domainId,
                FilterDomainName = filterName,
                StartDate = start,
                EndDate = end
            };
        }

        public async Task<ActivityCreateViewModel> GetCreateViewModelAsync()
        {
            var domains = await _domainRepo.GetAllAsync();
            return new ActivityCreateViewModel
            {
                Date = DateTime.Today,
                LifeDomainOptions = domains.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.TurkishName
                }).ToList()
            };
        }

        public async Task<ActivityEditViewModel?> GetEditViewModelAsync(int id, string userId)
        {
            var activity = await _activityRepo.GetByIdAsync(id);
            if (activity == null || activity.UserId != userId) return null;

            var domains = await _domainRepo.GetAllAsync();
            return new ActivityEditViewModel
            {
                Id = activity.Id,
                Title = activity.Title,
                Description = activity.Description,
                Date = activity.Date,
                EndDate = activity.EndDate,
                DurationMinutes = activity.DurationMinutes,
                LifeDomainId = activity.LifeDomainId,
                LifeDomainOptions = domains.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.TurkishName,
                    Selected = d.Id == activity.LifeDomainId
                }).ToList()
            };
        }

        public async Task<bool> CreateAsync(ActivityCreateViewModel model, string userId)
        {
            var activity = new Activity
            {
                Title = model.Title,
                Description = model.Description,
                Date = model.Date,
                EndDate = model.EndDate,
                DurationMinutes = model.DurationMinutes,
                LifeDomainId = model.LifeDomainId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            await _activityRepo.CreateAsync(activity);
            return true;
        }

        public async Task<bool> UpdateAsync(ActivityEditViewModel model, string userId)
        {
            var activity = await _activityRepo.GetByIdAsync(model.Id);
            if (activity == null || activity.UserId != userId) return false;

            activity.Title = model.Title;
            activity.Description = model.Description;
            activity.Date = model.Date;
            activity.EndDate = model.EndDate;
            activity.DurationMinutes = model.DurationMinutes;
            activity.LifeDomainId = model.LifeDomainId;

            await _activityRepo.UpdateAsync(activity);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var activity = await _activityRepo.GetByIdAsync(id);
            if (activity == null || activity.UserId != userId) return false;

            await _activityRepo.DeleteAsync(id);
            return true;
        }
    }
}
