using Microsoft.EntityFrameworkCore;
using portal_job_FN.Data;
using portal_job_FN.Models;

namespace portal_job_FN.Repositories
{
    public class EFLocationRepository : ILocationRepository
    {
        private readonly ApplicationDbContext _context;
        public EFLocationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<JobLocation>> GetAllAsync()
        {
            return await _context.job_location.ToListAsync();
        }
    }
}
