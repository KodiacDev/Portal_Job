using portal_job_FN.Models;

namespace portal_job_FN.Repositories
{
    public interface IPostJobRepository
    {
        Task<IEnumerable<PostJob>> GetAllAsync();
        Task<IEnumerable<PostJob>> GetAllByCompanyIdAsync(string id);
        Task<IEnumerable<PostJob>>  GetAllRelatedJobsById(string jobName, int id);
        Task<PostJob> GetByIdAsync(int id);
        Task AddAsync(PostJob post_Job);
        Task UpdateAsync(PostJob post_Job);
        Task DeleteAsync(int id);

    }
}
