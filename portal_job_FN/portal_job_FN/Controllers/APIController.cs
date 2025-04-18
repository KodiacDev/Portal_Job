using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portal_job_FN.Data;
using portal_job_FN.Dto;
using portal_job_FN.Models;
using portal_job_FN.Repositories;

namespace portal_job_FN.Controllers
{
    [Route("api/Jobs")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly IPostJobRepository _postJob;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVnPayRepository _vpnPayRepository;
        public APIController(IPostJobRepository postJob,
               ApplicationDbContext context,
               UserManager<ApplicationUser> userManager,
               IVnPayRepository vpnPayRepository)
        {
            _postJob = postJob;
            _context = context;
            _userManager = userManager;
            _vpnPayRepository = vpnPayRepository;
        }

        // GET: api/Jobs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostJobDto>>> GetJobs()
        {
            var jobs = await _postJob.GetAllJobAPIAsync();
            if (jobs == null || !jobs.Any())
            {
                return NotFound();
            }
            return Ok(jobs);
        }


        // GET: api/Jobs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostJob>> GetJob(int id)
        {
            var job = await _postJob.GetByIdAsync(id);

            if (job == null)
            {
                return BadRequest();
            }

            return job;
        }


        // GET: api/Jobs/MonthlyStats
        [HttpGet("MonthlyStats")]
        public async Task<ActionResult<IEnumerable<int>>> GetJobStatsByMonth()
        {
            var jobs = await _postJob.GetAllJobAPIAsync();
            var find_company = await _userManager.GetUserAsync(User);
            if (find_company == null)
            {
                return NotFound("Chưa đăng nhập");
            }

            if (jobs == null || !jobs.Any())
            {
                return NotFound("No jobs found.");
            }

            // Tạo danh sách 12 tháng với giá trị mặc định là 0
            var monthlyJobCounts = Enumerable.Range(1, 12).Select(month => 0).ToList();

            // Lấy thống kê số lượng công việc theo tháng
            var monthlyStats = jobs
                .Where(job => job.created_at.HasValue && job.created_at.Value.Year == 2024)
                .GroupBy(job => job.created_at.Value.Month)
                .Select(group => new
                {
                    Month = group.Key, // Tháng (1 -> 12)
                    JobCount = group.Count() // Số lượng công việc
                })
                .ToList();

            // Cập nhật danh sách 12 tháng với dữ liệu thực tế từ monthlyStats
            foreach (var stat in monthlyStats)
            {
                monthlyJobCounts[stat.Month - 1] = stat.JobCount;
            }

            // Trả về danh sách số lượng công việc theo từng tháng
            return Ok(monthlyJobCounts);
        }


        // GET: api/Jobs/MonthlyStatsCompany
        [HttpGet("MonthlyStatsCompany")]
        public async Task<ActionResult<IEnumerable<int>>> GetJobStatsByMonthCompany()
        {
            var jobs = await _postJob.GetAllJobAPIAsync();
            var find_company = await _userManager.GetUserAsync(User);

            if (find_company == null)
            {
                return NotFound("Chưa đăng nhập");
            }

            if (jobs == null || !jobs.Any())
            {
                return NotFound("Không tìm thấy công việc nào.");
            }

            // Lọc công việc theo công ty dựa trên id của người dùng đăng nhập
            var companyJobs = jobs
                .Where(job => job.companyId == find_company.Id) // Lọc công việc theo công ty
                .ToList();

            if (!companyJobs.Any())
            {
                return NotFound("Không tìm thấy công việc nào của công ty bạn.");
            }

            // Tạo danh sách 12 tháng với giá trị mặc định là 0
            var monthlyJobCounts = Enumerable.Range(1, 12).Select(month => 0).ToList();

            // Lấy thống kê theo tháng trong năm 2024
            var monthlyStats = companyJobs
                .Where(job => job.created_at.HasValue && job.created_at.Value.Year == 2024) // Kiểm tra năm
                .GroupBy(job => job.created_at.Value.Month) // Nhóm theo tháng
                .Select(group => new
                {
                    Month = group.Key, // Tháng (1 -> 12)
                    JobCount = group.Count() // Số lượng công việc trong tháng đó
                })
                .ToList();

            // Cập nhật danh sách 12 tháng với dữ liệu thực tế từ monthlyStats
            foreach (var stat in monthlyStats)
            {
                monthlyJobCounts[stat.Month - 1] = stat.JobCount;
            }

            // Trả về danh sách số lượng công việc theo từng tháng
            return Ok(monthlyJobCounts);
        }

        // GET: api/Jobs/MonthlyDeposits
        [HttpGet("MonthlyDeposits")]
        public async Task<ActionResult<IEnumerable<int>>> GetMonthlyDeposits()
        {
            // Giả định bạn có một dịch vụ hoặc lớp gọi là _transactionService
            var transactions = await _vpnPayRepository.GetAllHistoryPayByAdmin(); // Lấy tất cả giao dịch

            // Kiểm tra xem dữ liệu có tồn tại không
            if (transactions == null || !transactions.Any())
            {
                return NotFound("Không có giao dịch nào.");
            }

            // Lọc giao dịch trong năm 2024 và nhóm theo tháng
            var monthlyDeposits = transactions
                .Where(t => t.create_at.HasValue && t.create_at.Value.Year == 2024) // Chỉ lấy năm 2024
                .GroupBy(t => t.create_at.Value.Month) // Nhóm theo tháng
                .Select(group => new
                {
                    Month = group.Key, // Tháng
                    TotalAmount = group.Sum(t => t.Amount) // Tổng số tiền đã nạp trong tháng
                })
                .OrderBy(stat => stat.Month) // Sắp xếp theo tháng
                .ToList();

            // Tạo mảng số tiền nạp theo tháng, nếu tháng không có giao dịch thì trả về 0
            var monthlyAmounts = Enumerable.Range(1, 12).Select(month =>
                monthlyDeposits.FirstOrDefault(m => m.Month == month)?.TotalAmount ?? 0
            ).ToList();

            // Trả về mảng số tiền đã nạp theo tháng
            return Ok(monthlyAmounts);
        }
        // GET: api/Jobs/MonthlyDepositsCompany
        [HttpGet("MonthlyDepositsCompany")]
        public async Task<ActionResult<IEnumerable<int>>> GetMonthlyDepositsCompany()
        {
            var transactions = await _vpnPayRepository.GetAllHistoryPayByAdmin(); // Lấy tất cả giao dịch
            var find_company = await _userManager.GetUserAsync(User); // Lấy thông tin công ty đang đăng nhập

            if (find_company == null)
            {
                return NotFound("Chưa đăng nhập");
            }

            if (transactions == null || !transactions.Any())
            {
                return NotFound("Không có giao dịch nào.");
            }

            // Lọc giao dịch theo công ty (bằng companyId) và năm 2024
            var companyTransactions = transactions
                .Where(t => t.applicationUserId == find_company.Id && t.create_at.HasValue && t.create_at.Value.Year == 2024)
                .ToList();

            if (!companyTransactions.Any())
            {
                return NotFound("Không có giao dịch nào của công ty bạn.");
            }

            // Nhóm giao dịch theo tháng và tính tổng số tiền đã nạp trong mỗi tháng
            var monthlyDeposits = companyTransactions
                .GroupBy(t => t.create_at.Value.Month)
                .Select(group => new
                {
                    Month = group.Key, // Tháng
                    TotalAmount = group.Sum(t => t.Amount) // Tổng số tiền đã nạp trong tháng
                })
                .OrderBy(stat => stat.Month) // Sắp xếp theo tháng
                .ToList();

            // Tạo mảng số tiền nạp theo tháng, nếu tháng không có giao dịch thì trả về 0
            var monthlyAmounts = Enumerable.Range(1, 12).Select(month =>
                monthlyDeposits.FirstOrDefault(m => m.Month == month)?.TotalAmount ?? 0
            ).ToList();

            // Trả về mảng số tiền đã nạp theo tháng
            return Ok(monthlyAmounts);
        }




    }
}
