using portal_job_FN.Models;

namespace portal_job_FN.Repositories
{
    public interface IUniversity
    {
        Task<IEnumerable<University>> GetAllAsync();
    }
}
