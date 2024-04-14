using Microsoft.EntityFrameworkCore;
using portal_job_FN.Data;
using portal_job_FN.Models;

namespace portal_job_FN.Repositories
{
    public class EFUniversityRepository : IUniversity
    {
        private readonly ApplicationDbContext _context;
        public EFUniversityRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<University>> GetAllAsync()
        {
            return await _context.universities.ToListAsync();
        }
    }
}
