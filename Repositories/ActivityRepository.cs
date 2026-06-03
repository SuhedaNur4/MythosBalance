using Microsoft.EntityFrameworkCore;
using MythosBalance.Data;
using MythosBalance.Models;

namespace MythosBalance.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly ApplicationDbContext _context;

        public ActivityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Activity>> GetByUserIdAsync(string userId)
        {
            return await _context.Activities
                .Include(a => a.LifeDomain)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }

        public async Task<List<Activity>> GetByUserIdFilteredAsync(string userId, int? domainId, DateTime? start, DateTime? end)
        {
            var query = _context.Activities
                .Include(a => a.LifeDomain)
                .Where(a => a.UserId == userId);

            if (domainId.HasValue)
                query = query.Where(a => a.LifeDomainId == domainId.Value);

            if (start.HasValue)
                query = query.Where(a => a.Date >= start.Value);

            if (end.HasValue)
                query = query.Where(a => a.Date <= end.Value);

            return await query.OrderByDescending(a => a.Date).ToListAsync();
        }

        public async Task<Activity?> GetByIdAsync(int id)
        {
            return await _context.Activities
                .Include(a => a.LifeDomain)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Activity> CreateAsync(Activity activity)
        {
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task UpdateAsync(Activity activity)
        {
            _context.Activities.Update(activity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity != null)
            {
                _context.Activities.Remove(activity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<DateTime?> GetLastActivityDateByDomainAsync(string userId, int domainId)
        {
            return await _context.Activities
                .Where(a => a.UserId == userId && a.LifeDomainId == domainId)
                .OrderByDescending(a => a.Date)
                .Select(a => (DateTime?)a.Date)
                .FirstOrDefaultAsync();
        }

        public async Task<int> CountByUserAsync(string userId)
        {
            return await _context.Activities.CountAsync(a => a.UserId == userId);
        }

        public async Task<int> CountThisWeekByUserAsync(string userId)
        {
            var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            return await _context.Activities
                .CountAsync(a => a.UserId == userId && a.Date >= startOfWeek);
        }

        public async Task<int> CountThisMonthByUserAsync(string userId)
        {
            var startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            return await _context.Activities
                .CountAsync(a => a.UserId == userId && a.Date >= startOfMonth);
        }

        public async Task<Dictionary<int, int>> GetCountsByDomainAsync(string userId)
        {
            return await _context.Activities
                .Where(a => a.UserId == userId)
                .GroupBy(a => a.LifeDomainId)
                .Select(g => new { DomainId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.DomainId, x => x.Count);
        }

        public async Task<Dictionary<int, int>> GetCountsByDomainThisMonthAsync(string userId)
        {
            var startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            return await _context.Activities
                .Where(a => a.UserId == userId && a.Date >= startOfMonth)
                .GroupBy(a => a.LifeDomainId)
                .Select(g => new { DomainId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.DomainId, x => x.Count);
        }
    }
}
