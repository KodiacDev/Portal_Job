using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using portal_job_FN.Data;
using portal_job_FN.Models;
using portal_job_FN.Repositories;
using portal_job_FN.Session;
using System.Security.Claims;

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
        // GET: post_job
        public async Task<IActionResult> Index()
        {
            var post_jobs = _post_job.GetAllAsync();
            return View(await post_jobs);
        }

        // GET: Company/view_post
        public async Task<IActionResult> view_post()
        {
            var post_jobs = _post_job.GetAllAsync();
            return View(await post_jobs);
        }


        // GET: Company/Home/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post_job = await _post_job.GetByIdAsync(id);
            if (post_job == null)
            {
                return NotFound();
            }

            return View(post_job);
        }

        // GET: Company/Home/Create
        public async Task<IActionResult> Create_post_job()
        {
            var locations = await _locationRepository.GetAllAsync();
            var experiences = await _experience.GetAllAsync();
            var majors = await _major.GetAllAsync();
            ViewBag.Locations = new SelectList(locations, "Id", "province_name");
            ViewBag.Experiences = new SelectList(experiences, "Id", "experience_name");
            ViewBag.Majors = new SelectList(majors, "Id", "major_name");
            return View();
        }

        // POST: Company/Home/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,job_name,job_description,required_skill,benefit,employmentType,salary_min,salary_max,detail_location,apply_date,location_id,experience_id,major_id")] PostJob post_job)
        {
            if (ModelState.IsValid)
            {
                var find_user = await _userManager.GetUserAsync(User);
                if (find_user == null)
                {
                    /*return NotFound();*/
                    return View("Thiếu Id user");
                }
                post_job.applicationUserId = find_user.Id;
                post_job.create_at = DateTime.Now;
                post_job.update_at = DateTime.Now;
                post_job.is_active = 1;
                await _post_job.AddAsync(post_job);
                return RedirectToAction(nameof(Index));
            }
            return View(post_job);
        }

        // GET: Company/Home/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var locations = await _locationRepository.GetAllAsync();
            var experiences = await _experience.GetAllAsync();
            var majors = await _major.GetAllAsync();
            if (id == null)
            {
                ViewBag.Locations = new SelectList(locations, "Id", "province_name");
                ViewBag.Experiences = new SelectList(experiences, "Id", "experience_name");
                ViewBag.Majors = new SelectList(majors, "Id", "major_name");
                return NotFound();
            }

            var post_job = await _context.post_Jobs.FindAsync(id);
            if (post_job == null)
            {
                ViewBag.Locations = new SelectList(locations, "Id", "province_name");
                ViewBag.Experiences = new SelectList(experiences, "Id", "experience_name");
                ViewBag.Majors = new SelectList(majors, "Id", "major_name");
                return NotFound();
            }
            ViewBag.Locations = new SelectList(locations, "Id", "province_name");
            ViewBag.Experiences = new SelectList(experiences, "Id", "experience_name");
            ViewBag.Majors = new SelectList(majors, "Id", "major_name");
            return View(post_job);
        }
        [HttpGet]
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
        }

        // POST: Company/Home/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,job_name,job_description,required_skill,benefit,employmentType,salary_min,salary_max,detail_location,create_at,update_at,apply_date,is_active,location_id,experience_id,application_user,major_id")] PostJob post_job)
        {
            if (id != post_job.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _post_job.UpdateAsync(post_job);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!post_jobExists(post_job.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(post_job);
        }

        // GET: Company/Home/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post_job = await _post_job.GetByIdAsync(id);
            if (post_job == null)
            {
                return NotFound();
            }

            return View(post_job);
        }

        // POST: Company/Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post_job = await _post_job.GetByIdAsync(id);
            if (post_job != null)
            {
                await _post_job.DeleteAsync(post_job.Id);
            }
            return RedirectToAction(nameof(Index));
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



        private bool post_jobExists(int id)
        {
            return _context.post_Jobs.Any(e => e.Id == id);
        }
    }
}
