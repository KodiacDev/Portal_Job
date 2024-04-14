using portal_job_FN.Models;

namespace portal_job_FN.Repositories
{
    public interface IEducationRepository
    {
        Task<IEnumerable<Education>> GetAllAsync();
        Task<Education> GetByIdAsync(int id);
        Task AddAsync(Education education);
        Task UpdateAsync(Education education);
        Task DeleteAsync(int id);
    }
}
