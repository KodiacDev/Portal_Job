using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using portal_job_FN.Data;
using portal_job_FN.Models;
using portal_job_FN.Repositories;
using portal_job_FN.Session;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace portal_job_FN.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IExperienceRepository _experience;
        private readonly IMajorRepository _major;
        private readonly ApplicationDbContext _context;
        private readonly IPostJobRepository _post_job;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplyJobRepository _apply_job;
        public HomeController(ApplicationDbContext context,
           ILocationRepository locationRepository,
           IExperienceRepository experience,
           IMajorRepository major,
           IPostJobRepository post_job,
           UserManager<ApplicationUser> userManager,
           IApplyJobRepository apply_job)
        {
            _context = context;
            _locationRepository = locationRepository;
            _experience = experience;
            _major = major;
            _post_job = post_job;
            _userManager = userManager;
            _apply_job = apply_job;

        }

        public async Task<IActionResult> Index(int pageNumber = 1)
        {

            int pageSize = 10;
            //Lay ds kinh nghiem, dia diem
            var exps = await _experience.GetAllAsync();
            var locations = await _locationRepository.GetAllAsync();
            ViewBag.Locations = new SelectList(locations, "Id", "province_name");
            ViewBag.Exps = new SelectList(exps, "Id", "experience_name");
            IQueryable<PostJob> query = _context.post_Jobs
                .Include(b => b.job_Location)
                .Include(b => b.major)
                .Include(b => b.applyJobs)
                .Include(b => b.experience)
                .Include(b => b.applicationUser);
            var paginatedPostJobs = await PaginatedList<PostJob>.CreateAsync(query, pageNumber, pageSize);
            return View(paginatedPostJobs);
        }
        public IActionResult About1()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> SearchProducts(string job_name, string location, string experience,string salary, int pageNumber = 1)
        {
            try
            {
                var jobs = _context.post_Jobs.AsQueryable();

                if (!string.IsNullOrEmpty(job_name))
                {
                    jobs = jobs.Where(p => p.job_name.Contains(job_name));
                }

                if (!string.IsNullOrEmpty(salary))
                {
                    switch (salary)
                    {
                        case "Dưới 10 triệu":
                            jobs = jobs.Where(p => p.salary_min < 10); // Thí dụ: Dưới 10 triệu
                            break;
                        case "10 - 15 triệu":
                            jobs = jobs.Where(p => p.salary_min >= 10 && p.salary_max <= 15);
                            break;
                        case "15 - 20 triệu":
                            jobs = jobs.Where(p => p.salary_min >= 15 && p.salary_max <= 20); 
                            break;
                        case "20 - 25 triệu":
                            jobs = jobs.Where(p => p.salary_min >= 20 && p.salary_max <= 25);
                            break;
                        case "25 - 30 triệu":
                            jobs = jobs.Where(p => p.salary_min >= 25 && p.salary_max <= 30); 
                            break;
                        case "30 - 35 triệu":
                            jobs = jobs.Where(p => p.salary_min >= 30 && p.salary_max <= 35); 
                            break;
                        case "35 - 40 triệu":
                            jobs = jobs.Where(p => p.salary_min >= 35 && p.salary_max <= 40);
                            break;
                        case "Trên 50 triệu":
                            jobs = jobs.Where(p => p.salary_max > 50); 
                            break;
                        default:
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(location))
                {
                    jobs = jobs.Where(p => p.job_Location.province_name.Contains(location));
                }

                if (!string.IsNullOrEmpty(experience))
                {
                    jobs = jobs.Where(p => p.experience.experience_name.Contains(experience));
                }

                int pageSize = 10;

                var paginatedJobs = await PaginatedList<PostJob>.CreateAsync(jobs
                    .Include(b => b.job_Location)
                    .Include(b => b.major)
                    .Include(b => b.applyJobs)
                    .Include(b => b.experience)
                    .Include(b => b.applicationUser), pageNumber, pageSize);

                return View(paginatedJobs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        public async Task<IActionResult> view_post()
        {
            var post_jobs = _post_job.GetAllAsync();
            return View(await post_jobs);
        }



        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var imagesUrls = _context.post_Jobs_images.Where(p => p.post_Job.Id == id).Select(p => p.post_image_url).ToList();
            ViewBag.imagesUrls = imagesUrls;
            if (imagesUrls != null && imagesUrls.Any())
            {
                ViewBag.imagesUrls = imagesUrls;
            }
            else
            {
                ViewBag.imagesUrls = new List<string>(); // Gán một danh sách trống nếu không có ảnh nào
            }

            var post_job = await _post_job.GetByIdAsync(id);
            if (post_job == null)
            {
                return NotFound();
            }

            return View(post_job);
        }

    
  /*      [HttpGet]
        public async Task<IActionResult> Apply_job(int Id)
        {
            var post_jobs = await _post_job.GetByIdAsync(Id);
            if(post_jobs == null)
            {
                return NotFound(Id);
            }
            ViewBag.post_jobs = post_jobs;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Apply_job(int Id, [Bind("FullName,Email,cover_letter")] ApplyJob apply_job, IFormFile url_cv)
        {
            var find_user = await _userManager.GetUserAsync(User);
            if (find_user == null)
            {
                *//*return NotFound();*//*
                return View("Thiếu Id user");
            }

            var post = await _post_job.GetByIdAsync(Id);
            if (post == null)
            {
                *//*    return NotFound();*//*
                return View("Thiếu Id postjob");
            }

            if (ModelState.IsValid)
            {
                if (url_cv != null && IsCVFile(url_cv) && IsFileSizeValid(url_cv))
                {
                    // Lưu hình CV
                    apply_job.url_cv = await SaveCV(url_cv);
                }
                else
                {
                    ModelState.AddModelError("CV", "Vui lòng chọn một tệp pdf hợp lệ và có kích thước nhỏ hơn 10MB.");
                }
                apply_job.post_JobId = post.Id;
                apply_job.application_userId = find_user.Id;
                apply_job.applicationUserId = post.applicationUserId;
                apply_job.create_at = DateTime.Now;
                apply_job.update_at = DateTime.Now;
                await _apply_job.AddAsync(apply_job);
                return View(apply_job);
            }
            else
            {
                return View("else");
            }
        }
        [HttpGet]
        public async Task<IActionResult> List_Apply()
        {
                var list_apply = _apply_job.GetAllAsync();
                return View(await list_apply);
        }
        [HttpGet]
        public async Task<IActionResult> details_apply(int id)
        {
            var detail_apply = _apply_job.GetByIdAsync(id);
            if(detail_apply == null)
            {
                return NotFound();
            }
            return View(await detail_apply);
        }*/


   /*     private bool IsCVFile(IFormFile file)
        {
            // Kiểm tra phần mở rộng của file có phải là pdf hay không
            var allowedExtensions = new[] { ".pdf" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(fileExtension);
        }

        //Hàm lưu cv
        private async Task<string> SaveCV(IFormFile url_cv)
        {
            try
            {
                //đảm bảo tên cv là duy nhất khi up
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(url_cv.FileName);
                var savePath = Path.Combine("wwwroot/cv", fileName); // Thay đổi đường dẫn theo cấu hình của bạn
                using (var fileStream = new FileStream(savePath, FileMode.Create))
                {
                    await url_cv.CopyToAsync(fileStream);
                }
                return "/cv/" + fileName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }*/
       /* //Ktra trong gio hang co post_job chua
        [HttpGet]
        public IActionResult CheckCartItem()
        {
            var jobId = "@job.Id";
            var cart = HttpContext.Session.GetObjectFromJson<JobCart>("Cart") ?? new JobCart();
            var cartItemIds = cart.Items.Select(item => item.PostJobId.ToString()).ToList();
            return Ok(cartItemIds.Contains(jobId));
        }

        private bool IsFileSizeValid(IFormFile file)
        {
            // Kiểm tra kích thước file không vượt quá 10MB
            var maxSize = 10 * 1024 * 1024; // 10MB
            return file.Length <= maxSize;
        }

*/

        private bool post_jobExists(int id)
        {
            return _context.post_Jobs.Any(e => e.Id == id);
        }



        //chuc nang tim kiem
      /*  [HttpGet]
        public IActionResult SearchProducts(string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                    return BadRequest("Search query is required!");

                var products = _context.Products
                    .Where(p => p.Name.Contains(query) || (p.Description != null && p.Description.Contains(query)))
                    .ToList();

                return View(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/





    }
}
