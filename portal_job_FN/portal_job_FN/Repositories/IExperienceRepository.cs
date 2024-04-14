using portal_job_FN.Models;

namespace portal_job_FN.Repositories
{
    public interface IExperienceRepository
    {
        Task<IEnumerable<Experience>> GetAllAsync();
    }
}
