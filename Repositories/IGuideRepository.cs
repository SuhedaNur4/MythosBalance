using MythosBalance.Models;

namespace MythosBalance.Repositories
{
    public interface IGuideRepository
    {
        Task<List<MythologyGuide>> GetAllAsync();
        Task<MythologyGuide?> GetByIdAsync(int id);
        Task<MythologyGuide?> GetByDomainIdAsync(int domainId);
        Task<MythologyGuide> CreateAsync(MythologyGuide guide);
        Task UpdateAsync(MythologyGuide guide);
        Task DeleteAsync(int id);
    }

    public interface ILifeDomainRepository
    {
        Task<List<LifeDomain>> GetAllAsync();
        Task<LifeDomain?> GetByIdAsync(int id);
    }
}
