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
        public async Task<int> GetTotalPostsCountAsync()
        {
            var count = await _context.post_Jobs
                .CountAsync(); // Đếm tổng số bài đăng trong cơ sở dữ liệu

            return count;
        }
        public async Task<int> GetPostsCountInOneMonthAsync()
        {
            var oneMonthAgo = DateTime.Now.AddMonths(-1);

            var count = await _context.post_Jobs
                .Where(b => b.create_at >= oneMonthAgo) // Lọc các bài đăng trong 1 tháng qua
                .CountAsync(); // Đếm số bài đăng trong 1 tháng

            return count;
        }
        public async Task<int> GetPostsCountInSixMonthsAsync()
        {
            var sixMonthsAgo = DateTime.Now.AddMonths(-6);

            var count = await _context.post_Jobs
                .Where(b => b.create_at >= sixMonthsAgo) // Lọc các bài đăng trong 6 tháng qua
                .CountAsync(); // Đếm số bài đăng trong 6 tháng

            return count;
        }
        public async Task<int> GetPostsCountInOneYearAsync()
        {
            var oneYearAgo = DateTime.Now.AddYears(-1);

            var count = await _context.post_Jobs
                .Where(b => b.create_at >= oneYearAgo) // Lọc các bài đăng trong 1 năm qua
                .CountAsync(); // Đếm số bài đăng trong 1 năm

            return count;
        }
        public async Task<int> GetPostsCountYearToDateAsync()
        {
            var startOfYear = new DateTime(DateTime.Now.Year, 1, 1);

            var count = await _context.post_Jobs
                .Where(b => b.create_at >= startOfYear) // Lọc các bài đăng từ đầu năm đến nay
                .CountAsync(); // Đếm số bài đăng từ đầu năm đến nay

            return count;
        }

        //Company
        public async Task<object> GetPostsCountByMonthAsync()
        {
            var currentYear = DateTime.Now.Year;
            var postCounts = new List<int>();
            var monthLabels = new List<string>();

            // Lặp qua 12 tháng trong năm
            for (int month = 1; month <= 12; month++)
            {
                var firstDayOfMonth = new DateTime(currentYear, month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1); // Ngày cuối cùng của tháng

                // Lọc các bài đăng trong tháng
                var count = await _context.post_Jobs
                    .Where(b => b.create_at >= firstDayOfMonth && b.create_at <= lastDayOfMonth)
                    .CountAsync();

                postCounts.Add(count); // Thêm số bài đăng vào danh sách

                // Thêm nhãn tháng vào danh sách
                monthLabels.Add(firstDayOfMonth.ToString("MMMM"));
            }

            // Tạo dữ liệu trả về
            var data = new
            {
                categories = monthLabels, // Các nhãn tháng
                series = new[]
                {
            new
            {
                name = "Ứng tuyển",
                data = postCounts
            }
        }
            };

            return data;
        }

















        public async Task<IEnumerable<PostJobDto>> GetAllJobAPIAsync()
        {
            // Nạp dữ liệu từ post_Jobs và liên kết với bảng images
            var applicationDbContext = await _context.post_Jobs
                .Include(b => b.experience) // Liên kết với experience
                .Include(b => b.applicationUser) // Liên kết với applicationUser (company_name)
                .Include(b => b.post_Job_Images) // Liên kết với bảng images (giả sử tên property là Images)
                .OrderByDescending(b => b.create_at) // Sắp xếp theo ngày đăng giảm dần
                .ToListAsync();

            // Ánh xạ từ PostJob sang PostJobDto
            var postJobDtos = applicationDbContext.Select(postJob => new PostJobDto
            {
                Id = postJob.Id,
                Title = postJob.job_name,
                experience = postJob.experience?.experience_name, // Giả sử experience có thuộc tính Name
                company_name = postJob.applicationUser?.company_name, // Giả sử applicationUser có thuộc tính CompanyName
                max_salary = postJob.salary_max?.ToString(), // Nếu MaxSalary là số, chuyển thành chuỗi
                created_at = postJob.create_at,
                companyId = postJob.applicationUserId,
                required_skill = postJob.required_skill,
                detail_location = postJob.detail_location,
                employmentType = postJob.employmentType,
                Description = postJob.job_description,
                benefit = postJob.benefit,
                image_Url = postJob.applicationUser.image_url,
                urlImages = postJob.post_Job_Images?.Select(img => img.post_image_url).ToList() // Trả về danh sách URL ảnh của bài viết
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
