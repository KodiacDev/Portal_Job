using Microsoft.EntityFrameworkCore;
using portal_job_FN.Data;
using portal_job_FN.Dto;
using portal_job_FN.Models;

namespace portal_job_FN.Repositories
{
    public class EFPostJobRepository : IPostJobRepository
    {
        private readonly ApplicationDbContext _context;
        public EFPostJobRepository(ApplicationDbContext context)
        {
            _context = context;
        }
		public async Task<IEnumerable<PostJob>> GetAllAsync()
		{
			// Bao gồm các liên kết và sắp xếp theo ngày đăng giảm dần
			var applicationDbContext = await _context.post_Jobs
				.Include(b => b.job_Location)
				.Include(b => b.major)
				.Include(b => b.applyJobs)
				.Include(b => b.experience)
				.Include(b => b.applicationUser)
				.OrderByDescending(b => b.create_at) // Sắp xếp theo ngày đăng giảm dần
				.ToListAsync();

			return applicationDbContext;
		}

        public async Task<IEnumerable<PostJobDto>> GetAllJobAPIAsync()
        {
            // Bao gồm các liên kết cần thiết và sắp xếp theo ngày đăng giảm dần
            var applicationDbContext = await _context.post_Jobs
                .Include(b => b.experience) // Liên kết với experience
                .Include(b => b.applicationUser) // Liên kết với applicationUser (company_name)
                .OrderByDescending(b => b.create_at) // Sắp xếp theo ngày đăng giảm dần
                .ToListAsync();

            // Ánh xạ từ PostJob sang PostJobDto
            var postJobDtos = applicationDbContext.Select(postJob => new PostJobDto
            {
                Title = postJob.job_name,
                experience = postJob.experience?.experience_name, // Giả sử experience có thuộc tính Name
                company_name = postJob.applicationUser?.company_name, // Giả sử applicationUser có thuộc tính CompanyName
                max_salary = postJob.salary_max?.ToString() // Nếu MaxSalary là số, chuyển thành chuỗi
            });

            return postJobDtos;
        }




        public async Task<IEnumerable<PostJob>> GetAllByCompanyIdAsync(string id)
        {
            //bao gồm danh mục, nếu không có sẽ ko ra danh mục
            var applicationDbContext = await _context.post_Jobs
                 .Include(b => b.job_Location)
                .Include(b => b.major)
                .Include(b => b.applyJobs)
                .Include(b => b.experience)
                .Include(b => b.applicationUser)
                .Where(b => b.applicationUser.Id == id)
                .ToListAsync();

            return applicationDbContext;
        }

		public async Task<IEnumerable<PostJob>> GetAllRelatedJobsById(string jobMajor, int excludeJobId)
		{
			var applicationDbContext = await _context.post_Jobs
				.Include(b => b.job_Location)
				.Include(b => b.major)
				.Include(b => b.applyJobs)
				.Include(b => b.experience)
				.Include(b => b.applicationUser)
				.Where(b => (string.IsNullOrEmpty(jobMajor) || (b.major != null && b.major.major_name == jobMajor)) // Kiểm tra nếu b.major null hoặc jobMajor không khớp
							&& b.Id != excludeJobId) // Loại bỏ công việc hiện tại theo ID
				.OrderByDescending(b => b.create_at) // Sắp xếp theo ngày đăng giảm dần
				.Take(5) // Lấy tối đa 5 công việc
				.ToListAsync();

			return applicationDbContext;
		}





		public async Task<PostJob> GetByIdAsync(int id)
        {
            var applicationDbContext = await _context.post_Jobs
                .Include(b => b.job_Location)
                .Include(b => b.major)
                .Include(b => b.experience)
                .Include(b => b.applyJobs)
                .Include(b => b.applicationUser)
                .ToListAsync();
            return await _context.post_Jobs.FindAsync(id);
        }

        public async Task AddAsync(PostJob post_Job)
        {
            _context.post_Jobs.Add(post_Job);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PostJob post_Job)
        {
            _context.post_Jobs.Update(post_Job);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var post_Job = await _context.post_Jobs.FindAsync(id);
            if (post_Job != null)
            {
                _context.post_Jobs.Remove(post_Job);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CountAllPostByIdCompany(string idCompany)
        {
            // Lọc các bài đăng thuộc công ty có mã idCompany
            var count = await _context.post_Jobs
                .Where(p => p.applicationUserId == idCompany)
                .CountAsync();

            return count; // Trả về tổng số bài đăng
        }


        public async Task<int> CountAllPostByAdmin()
        {
            // Đếm toàn bộ bài đăng trong hệ thống
            var count = await _context.post_Jobs.CountAsync();
            return count; // Trả về tổng số bài đăng
        }

    }
}
