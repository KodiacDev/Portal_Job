using Microsoft.EntityFrameworkCore;
using portal_job_FN.Data;
using portal_job_FN.Models;

namespace portal_job_FN.Repositories
{
    public class EFEducationRepository : IEducationRepository
    {
        private readonly ApplicationDbContext _context;
        public EFEducationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Education>> GetAllAsync()
        {
            //bao gồm danh mục, nếu không có sẽ ko ra danh mục
            var applicationDbContext = await _context.educations
                .Include(b => b.Major)
                .Include(b => b.university)
                .Include(b => b.applicationUser)
                .ToListAsync();

            return await _context.educations.ToListAsync();
        }
        public async Task<Education> GetByIdAsync(int id)
        {
            var applicationDbContext = await _context.educations
                .Include(b => b.Major)
                .Include(b => b.university)
                .Include(b => b.applicationUser)
                .ToListAsync();
            return await _context.educations.FindAsync(id);
        }

        public async Task AddAsync(Education education)
        {
            _context.educations.Add(education);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Education education)
        {
            _context.educations.Update(education);
            await _context.SaveChangesAsync();
        }

        public async Task? DeleteAsync(int id)
        {
            var education = await _context.educations.FindAsync(id);
            _context.educations.Remove(education);
            await _context.SaveChangesAsync();
        }

    }
}
