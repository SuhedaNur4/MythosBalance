using MythosBalance.Models;

namespace MythosBalance.Repositories
{
    public interface IActivityRepository
    {
        Task<List<Activity>> GetByUserIdAsync(string userId);
        Task<List<Activity>> GetByUserIdFilteredAsync(string userId, int? domainId, DateTime? start, DateTime? end);
        Task<Activity?> GetByIdAsync(int id);
        Task<Activity> CreateAsync(Activity activity);
        Task UpdateAsync(Activity activity);
        Task DeleteAsync(int id);
        Task<DateTime?> GetLastActivityDateByDomainAsync(string userId, int domainId);
        Task<int> CountByUserAsync(string userId);
        Task<int> CountThisWeekByUserAsync(string userId);
        Task<int> CountThisMonthByUserAsync(string userId);
        Task<Dictionary<int, int>> GetCountsByDomainAsync(string userId);
        Task<Dictionary<int, int>> GetCountsByDomainThisMonthAsync(string userId);
    }
}
