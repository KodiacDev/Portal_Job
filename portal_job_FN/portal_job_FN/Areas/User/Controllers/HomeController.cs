﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using portal_job_FN.Data;
using portal_job_FN.Models;
using portal_job_FN.Repositories;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
namespace portal_job_FN.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = SD.Role_User)]
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
                return NotFound();
            }

            // Kiểm tra nếu hạn nộp CV đã qua
            if (post_jobs.create_at.HasValue && post_jobs.apply_date.Value < DateTime.Now)
            {
                // Hiển thị thông báo hoặc redirect đến một trang khác
                return NotFound("Hạn nộp CV đã qua. Bạn không thể nộp CV nữa.");
            }

            ViewBag.post_jobs = post_jobs;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListApply()
        {
            var find_user = await _userManager.GetUserAsync(User);
            if (find_user != null)
            {
                
                var list_apply = await _apply_job.GetAllApplyByUserIdAsync(find_user.Id);
    /*            var infoCompany = await _post_job.GetByIdAsync(list_apply.)*/
                return View(list_apply);
            }
            else
            {
                return NotFound();
            }
  
        }
        [HttpGet]
        public async Task<IActionResult> DetailsApply(int id)
        {
            var applyJob = await _apply_job.GetByIdAsync(id);
            var infoUser = await _userManager.GetUserAsync(User);
            ViewBag.InfoUser = infoUser;
            return View(applyJob);
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
                    // Lưu CV
                    apply_job.url_cv = await SaveCV(url_cv);
                }
                else
                {
                    ModelState.AddModelError("url_cv", "Vui lòng chọn một tệp pdf hợp lệ và có kích thước nhỏ hơn 10MB.");
                    return View(apply_job);
                }
                apply_job.post_JobId = post.Id;
                apply_job.application_userId = post.applicationUserId;
                apply_job.applicationUserId = find_user.Id;
                apply_job.imageCompany = post.applicationUser?.image_url;
                apply_job.companyName = post.applicationUser?.company_name;
                apply_job.emailCompany = post.applicationUser?.Email;
                apply_job.contact_noCompany = post.applicationUser?.mobile_no;
                apply_job.create_at = DateTime.Now;
                apply_job.update_at = DateTime.Now;
                await _apply_job.AddAsync(apply_job);
                /*   return View(apply_job);*/
                return RedirectToAction(nameof(ListApply));
            }
            else
            {
                return BadRequest("Có gì đó không đúng");
            }
        }
        private bool IsCVFile(IFormFile file)
        {
            // Kiểm tra phần mở rộng của file có phải là pdf hay không
            var allowedExtensions = new[] { ".pdf" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return false;
            }

            // Kiểm tra nội dung của tệp tin để đảm bảo rằng đây là tệp tin PDF hợp lệ
            try
            {
                using (var reader = new PdfReader(file.OpenReadStream()))
                {
                    // Nếu không có lỗi khi mở tệp tin, và có ít nhất một trang trong tệp tin PDF
                    return reader.NumberOfPages > 0;
                }
            }
            catch
            {
                // Nếu có lỗi khi mở tệp tin hoặc tệp tin không có trang nào, trả về false
                return false;
            }
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
