using Microsoft.EntityFrameworkCore;
using MythosBalance.Data;
using MythosBalance.Models;

namespace MythosBalance.Repositories
{
    public class GuideRepository : IGuideRepository
    {
        private readonly ApplicationDbContext _context;

        public GuideRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MythologyGuide>> GetAllAsync()
        {
            return await _context.MythologyGuides
                .Include(g => g.LifeDomain)
                .Include(g => g.References)
                .OrderBy(g => g.LifeDomainId)
                .ToListAsync();
        }

        public async Task<MythologyGuide?> GetByIdAsync(int id)
        {
            return await _context.MythologyGuides
                .Include(g => g.LifeDomain)
                .Include(g => g.References)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<MythologyGuide?> GetByDomainIdAsync(int domainId)
        {
            return await _context.MythologyGuides
                .Include(g => g.References)
                .FirstOrDefaultAsync(g => g.LifeDomainId == domainId);
        }

        public async Task<MythologyGuide> CreateAsync(MythologyGuide guide)
        {
            _context.MythologyGuides.Add(guide);
            await _context.SaveChangesAsync();
            return guide;
        }

        public async Task UpdateAsync(MythologyGuide guide)
        {
            _context.MythologyGuides.Update(guide);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var guide = await _context.MythologyGuides.FindAsync(id);
            if (guide != null)
            {
                _context.MythologyGuides.Remove(guide);
                await _context.SaveChangesAsync();
            }
        }
    }

    public class LifeDomainRepository : ILifeDomainRepository
    {
        private readonly ApplicationDbContext _context;

        public LifeDomainRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LifeDomain>> GetAllAsync()
        {
            return await _context.LifeDomains
                .Include(d => d.MythologyGuide)
                .OrderBy(d => d.Id)
                .ToListAsync();
        }

        public async Task<LifeDomain?> GetByIdAsync(int id)
        {
            return await _context.LifeDomains
                .Include(d => d.MythologyGuide)
                .FirstOrDefaultAsync(d => d.Id == id);
        }
    }
}
