using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using portal_job_FN.Data;
using portal_job_FN.Models;
using portal_job_FN.Repositories;

namespace portal_job_FN.Areas.User.Controllers
{
    [Area("User")]
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
        [HttpGet]
        public async Task<IActionResult> Apply_job(int Id)
        {
            var post_jobs = await _post_job.GetByIdAsync(Id);
            if (post_jobs == null)
            {
                return NotFound(Id);
            }
            ViewBag.post_jobs = post_jobs;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ListApply()
        {
            var list_apply = _apply_job.GetAllAsync();
            return View(await list_apply);
        }
        [HttpGet]
        public async Task<IActionResult> DetailsApply(int id)
        {
            var detail_apply = _apply_job.GetByIdAsync(id);
            if (detail_apply == null)
            {
                return NotFound();
            }
            return View(await detail_apply);
        }
        [HttpPost]
        public async Task<IActionResult> Apply_job(int Id, [Bind("FullName,Email,cover_letter")] ApplyJob apply_job, IFormFile url_cv)
        {
            var find_user = await _userManager.GetUserAsync(User);
            if (find_user == null)
            {
                /*return NotFound();*/
                return View("Thiếu Id user");
            }

            var post = await _post_job.GetByIdAsync(Id);
            if (post == null)
            {
                /*    return NotFound();*/
                return View("Thiếu Id postjob");
            }

            if (ModelState.IsValid)
            {
                if (url_cv != null && IsCVFile(url_cv) && IsFileSizeValid(url_cv))
                {
                    // Lưu hình ảnh đại diện
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
        private bool IsCVFile(IFormFile file)
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
        }

        private bool IsFileSizeValid(IFormFile file)
        {
            // Kiểm tra kích thước file không vượt quá 10MB
            var maxSize = 10 * 1024 * 1024; // 10MB
            return file.Length <= maxSize;
        }



    }
}
