using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using portal_job_FN.Data;
using portal_job_FN.Models;
using portal_job_FN.Repositories;

namespace portal_job_FN.Areas.Company.Controllers
{
    [Area("Company")]
    [Authorize(Roles = SD.Role_Company)]

    public class HomeController : Controller
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IExperienceRepository _experience;
        private readonly IMajorRepository _major;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IPostJobRepository _post_job;
        private readonly IApplyJobRepository _apply_job;
        private readonly IVnPayRepository _vpnPayRepository;

        public HomeController(ApplicationDbContext context,
            ILocationRepository locationRepository,
            IExperienceRepository experience,
            IMajorRepository major,
            IPostJobRepository post_job,
            IVnPayRepository vpnPayRepository,
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
            _vpnPayRepository = vpnPayRepository;
        }

        private bool IsImageFile(IFormFile file)
        {
            // Kiểm tra phần mở rộng của file có phải là ảnh hay không
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            // Kiểm tra kiểu MIME của file để đảm bảo đó là ảnh
            var mimeType = file.ContentType.ToLowerInvariant();
            if (!mimeType.StartsWith("image/"))
            {
                return false;
            }

            return allowedExtensions.Contains(fileExtension);
        }

        //Hàm lưu ảnh
        private async Task<string> SaveImage(IFormFile image)
        {
            try
            {
                // Kiểm tra và tạo thư mục nếu không tồn tại
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Đảm bảo tên ảnh là duy nhất khi upload
                var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileNameWithoutExtension(image.FileName) + Path.GetExtension(image.FileName);
                var savePath = Path.Combine(directory, fileName);

                using (var fileStream = new FileStream(savePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                return "/img/" + fileName;
            }
            catch (Exception e)
            {
                // Ghi log chi tiết về lỗi
                Console.WriteLine($"Error while saving image: {e.Message}, {e.StackTrace}");
                throw; // Ném lại lỗi để có thể xử lý ở nơi khác
            }
        }

        private bool IsFileSizeValid(IFormFile file)
        {
            // Kiểm tra kích thước file không vượt quá 10MB
            var maxSize = 10 * 1024 * 1024; // 10MB
            return file.Length <= maxSize;
        }
        // GET: Company/Home
        public async Task<IActionResult> Index()
        {
            var find_company = await _userManager.GetUserAsync(User);
            //Tổng tiền nạp 
            ViewBag.TotalPayment = await _vpnPayRepository.TotalMoneyByCompanyId(find_company.Id);
            //Tổng ứng viên nôp cv
            ViewBag.CountAllJobSeekerByIdCompany = await _apply_job.CountAllJobSeekerByIdCompany(find_company.Id);
            //Tổng ứng viên nộp cv chưa được duyệt
            ViewBag.CountAllunapprovedByIdCompany = await _apply_job.CountAllunapprovedByIdCompany(find_company.Id);
            //Số lượt đăng bài còn lại
            ViewBag.CountPost = find_company.PostCount;
            //Tổng số lượng bài đã đăng
            ViewBag.CountPosted = await _post_job.CountAllPostByIdCompany(find_company.Id);
            if (find_company == null)
            {
                return NotFound("Chưa đăng nhập");
            }
            return View(find_company);
        }
        // GET: Company/Home
        public async Task<IActionResult> _Layout_Company()
        {
            var find_company = await _userManager.GetUserAsync(User);
            if (find_company == null)
            {
                return NotFound("Chưa đăng nhập");
            }
            ViewBag.Company = find_company;
            return View(find_company);
        }
        // GET: Company/view_post
        public async Task<IActionResult> view_post()
        {
            var find_user = await _userManager.GetUserAsync(User);
            if (find_user != null)
            {
                ViewBag.User = find_user;
                var post_jobs = await _post_job.GetAllByCompanyIdAsync(find_user.Id);
                return View(post_jobs);
            }
            else
            {
                return NotFound();
            }

        }

        public async Task<IActionResult> ViewListApplyJob()
        {
            var find_user = await _userManager.GetUserAsync(User);
            if (find_user != null)
            {

                var list_apply = await _apply_job.GetAllApplyByCompanyIdAsync(find_user.Id);
                return View(list_apply);
            }
            else
            {
                return NotFound();
            }
        }

       
     
        public async Task<IActionResult> DetailsApplyJob(int id)
        {
            var find_user = await _userManager.GetUserAsync(User);
            var applyJob = await _apply_job.GetByIdAsync(id);
            //Check nếu không phải là nhà tuyển dụng của ứng viên đó thì không xem được
            if (find_user.Id != applyJob.application_userId)
            {
                return Forbid();
            }
                return View(applyJob);
        }

        // GET: Company/Home/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var find_user = await _userManager.GetUserAsync(User);

       
            var post_job = await _post_job.GetByIdAsync(id);
            if (find_user.Id != post_job.applicationUserId)
            {
                return Forbid();
            }
            if (post_job == null)
            {
                return NotFound();
            }

            return View(post_job);
        }

        // GET: Company/Home/Create
      
        public async Task<IActionResult> CreatePostJob()
        {
            var find_company = await _userManager.GetUserAsync(User);
            if(find_company.PostCount <= 0)
            {
                return BadRequest("Hettien");
            }
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
        public async Task<IActionResult> Create([Bind("Id,job_name,job_description,required_skill,benefit,employmentType,salary_min,salary_max,detail_location,apply_date,job_LocationId,experienceId,majorId")] PostJob post_job, List<IFormFile> post_Job_Images)
        {
         /*   if (ModelState.IsValid)
            {*/
                //Lấy ra id công ty đăng baì
                var find_company = await _userManager.GetUserAsync(User);
                if (find_company != null)
                {
                    post_job.applicationUserId = find_company.Id;
                }
                if(find_company.PostCount <= 0)
                {
                return BadRequest("Hettien");
                }
                if (post_Job_Images != null)
                {
                    // Lưu danh sách ảnh postJob
                    post_job.post_Job_Images = new List<PostJobImage>();
                    foreach (var imageUrl in post_Job_Images)
                    {
                        var savedImage = await SaveImage(imageUrl);

                        // Tạo một đối tượng PostJobImage mới
                        var postJobImage = new PostJobImage
                        {
                            post_image_url = savedImage,
                            post_JobId = post_job.Id,
                        };

                        // Thêm đối tượng PostJobImage mới vào danh sách post_job.post_Job_Images
                        post_job.post_Job_Images.Add(postJobImage);
                    }
                }
                else
                {
                    ModelState.AddModelError("ImageUrl", "Vui lòng chọn một tệp hình ảnh hợp lệ và có kích thước nhỏ hơn 5MB.");
                    var locations = await _locationRepository.GetAllAsync();
                    var experiences = await _experience.GetAllAsync();
                    var majors = await _major.GetAllAsync();
                    ViewBag.Locations = new SelectList(locations, "Id", "province_name");
                    ViewBag.Experiences = new SelectList(experiences, "Id", "experience_name");
                    ViewBag.Majors = new SelectList(majors, "Id", "major_name");
                    return View(post_job);
                }
                post_job.create_at = DateTime.Now;
                post_job.update_at = DateTime.Now;
                post_job.required_skill = post_job.required_skill?.Replace("\r\n", "\n");
                post_job.job_description = post_job.job_description?.Replace("\r\n", "\n");
                post_job.benefit = post_job.benefit?.Replace("\r\n", "\n");
                post_job.is_active = 1;
                find_company.PostCount = find_company.PostCount - 1;
                await _userManager.UpdateAsync(find_company);
                await _post_job.AddAsync(post_job);
                return RedirectToAction(nameof(view_post));
     /*       }
            return View(post_job);*/
        }

        // GET: Company/Home/Edit/5
        public async Task<IActionResult> Edit(int? id, List<IFormFile> image_urls)
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

        // POST: Company/Home/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,job_name,job_description,required_skill,benefit,employmentType,salary_min,salary_max,detail_location,apply_date,job_LocationId,experienceId,majorId")] PostJob post_job, List<IFormFile> post_Job_Images)
        {
            if (id != post_job.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var find_company = await _userManager.GetUserAsync(User);
                    if (find_company != null)
                    {
                        if (post_Job_Images != null)
                        {
                            // Lưu danh sách ảnh postJob
                            post_job.post_Job_Images = new List<PostJobImage>();
                            foreach (var imageUrl in post_Job_Images)
                            {
                                var savedImage = await SaveImage(imageUrl);

                                // Tạo một đối tượng PostJobImage mới
                                var postJobImage = new PostJobImage
                                {
                                    post_image_url = savedImage,
                                    post_JobId = post_job.Id,
                                };

                                // Thêm đối tượng PostJobImage mới vào danh sách post_job.post_Job_Images
                                post_job.post_Job_Images.Add(postJobImage);
                            }
                        }
                        //Lưu thông tin cty đăng bài
                        post_job.applicationUserId = find_company.Id;
                        post_job.required_skill = post_job.required_skill?.Replace("\r\n", "\n");
                        post_job.job_description = post_job.job_description?.Replace("\r\n", "\n");
                        post_job.benefit = post_job.benefit?.Replace("\r\n", "\n");
                        post_job.create_at = DateTime.Now;
                        post_job.is_active = 1;
                        post_job.update_at = DateTime.Now;
                        await _post_job.UpdateAsync(post_job);
                    }
                    else
                    {
                        return NotFound();
                    }
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
                return RedirectToAction(nameof(view_post));
            }
            return View(post_job);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateFeedback(int id, [Bind("Feedback")] ApplyJob applyJob)
        {
            var applyjob = await _apply_job.GetByIdAsync(id);
            if (id != applyjob.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                    if (applyJob != null)
                    {
                        applyjob.Feedback = applyJob.Feedback;
                        await _apply_job.UpdateAsync(applyjob);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            return RedirectToAction("ViewListApplyJob", "Home", new { area = "Company" });

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

        [HttpGet]
        public async Task<IActionResult> DeletePostJob(int id)
        {
            var post_job = await _post_job.GetByIdAsync(id);
            if (post_job != null)
            {
                await _post_job.DeleteAsync(post_job.Id);
            }
            return RedirectToAction(nameof(view_post));
        }

        private bool post_jobExists(int id)
        {
            return _context.post_Jobs.Any(e => e.Id == id);
        }
        private bool ApplyJobExists(int id)
        {
            return _context.apply_Jobs.Any(e => e.Id == id);
        }
    }
}
