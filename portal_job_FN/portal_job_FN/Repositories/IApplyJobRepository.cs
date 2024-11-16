using portal_job_FN.Models;

namespace portal_job_FN.Repositories
{
    public interface IApplyJobRepository
    {
        Task<IEnumerable<ApplyJob>> GetAllAsync();
        Task<ApplyJob> GetByIdAsync(int id);
        Task<IEnumerable<ApplyJob>> GetAllApplyByUserIdAsync(string id);
        Task<IEnumerable<ApplyJob>> GetAllApplyByCompanyIdAsync(string id);
        Task AddAsync(ApplyJob apply_Job);
        Task UpdateAsync(ApplyJob apply_Job);
        Task DeleteAsync(int id);
    }
}
