using portal_job_FN.Models;

namespace portal_job_FN.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<IEnumerable<ApplicationUser>> GetAllCompanyAsync();
        Task<ApplicationUser> GetByIdAsync(string id);
        Task DeleteAsync(string id);
    }
}
