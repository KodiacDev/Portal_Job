using Microsoft.EntityFrameworkCore;
using portal_job_FN.Data;
using portal_job_FN.Models;

namespace portal_job_FN.Repositories
{
    public class EFApply_job : IApplyJobRepository
    {
        private readonly ApplicationDbContext _context;
        public EFApply_job(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ApplyJob>> GetAllAsync()
        {
            //bao gồm danh mục, nếu không có sẽ ko ra danh mục
            var applicationDbContext = await _context.apply_Jobs
                .Include(b => b.post_Job)
                .Include(b => b.applicationUser)
                .ToListAsync();

            return await _context.apply_Jobs.ToListAsync();
        }
        public async Task<ApplyJob> GetByIdAsync(int id)
        {
            var applicationDbContext = await _context.apply_Jobs
                .Include(b => b.post_Job)
                .Include(b => b.applicationUser)
                .ToListAsync();
            return await _context.apply_Jobs.FindAsync(id);
        }

        public async Task AddAsync(ApplyJob apply_Job)
        {
            _context.apply_Jobs.Add(apply_Job);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ApplyJob apply_Job)
        {
            _context.apply_Jobs.Update(apply_Job);
            await _context.SaveChangesAsync();
        }

        public async Task? DeleteAsync(int id)
        {
            var apply_Job = await _context.apply_Jobs.FindAsync(id);
            _context.apply_Jobs.Remove(apply_Job);
            await _context.SaveChangesAsync();
        }

    }
}
