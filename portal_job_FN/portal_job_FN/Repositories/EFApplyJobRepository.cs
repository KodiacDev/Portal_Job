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
				.OrderByDescending(b => b.create_at) // Sắp xếp theo ngày đăng giảm dần
				.ToListAsync();

            return await _context.apply_Jobs.ToListAsync();
        }


        public async Task<IEnumerable<ApplyJob>> GetAllApplyByUserIdAsync(string id)
        {
            //bao gồm danh mục, nếu không có sẽ ko ra danh mục
            var applicationDbContext = await _context.apply_Jobs
                .Include(b => b.post_Job)
                .Include(b => b.applicationUser)
                .Where(b => b.applicationUser.Id == id)
				.OrderByDescending(b => b.create_at) // Sắp xếp theo ngày đăng giảm dần
				.ToListAsync();

            return applicationDbContext;
        }

        public async Task<IEnumerable<ApplyJob>> GetAllApplyByCompanyIdAsync(string id)
        {
            //bao gồm danh mục, nếu không có sẽ ko ra danh mục
            var applicationDbContext = await _context.apply_Jobs
                .Include(b => b.post_Job)
                .Include(b => b.applicationUser)
                .Where(b => b.post_Job.applicationUser.Id == id)
				.OrderByDescending(b => b.create_at) // Sắp xếp theo ngày đăng giảm dần
				.ToListAsync();

            return applicationDbContext;
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

        public async Task<int> CountAllJobSeekerByIdCompany(string idCompany)
        {
            // Lọc danh sách các đơn ứng tuyển thuộc công ty có idCompany
            var count = await _context.apply_Jobs
                .Where(j => j.application_userId == idCompany)
                .CountAsync();

            return count; // Trả về tổng số ứng viên
        }

        public async Task<int> CountAllJobSeekerByIdAdmin()
        {
            // Đếm tất cả các đơn ứng tuyển trong hệ thống
            var count = await _context.apply_Jobs.CountAsync();
            return count; // Trả về tổng số ứng viên
        }

        public async Task<int> CountAllunapprovedByIdCompany(string idCompany)
        {
            // Lọc các đơn ứng tuyển của công ty có `feedback == null`
            var count = await _context.apply_Jobs
                .Where(j => j.application_userId == idCompany && j.Feedback == null)
                .CountAsync();

            return count; // Trả về số lượng chưa được duyệt
        }


        public async Task<int> CountAllunapprovedByIdAdmin()
        {
            // Đếm tất cả các đơn ứng tuyển trong hệ thống có `feedback == null`
            var count = await _context.apply_Jobs
                .Where(j => j.Feedback == null)
                .CountAsync();

            return count; // Trả về số lượng chưa được duyệt
        }

    }
}
