using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using portal_job_FN.Data;
using portal_job_FN.Models;
using portal_job_FN.Repositories;

namespace portal_job_FN.Areas.User.Controllers
{
    [Area("User")]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEducationRepository _educationRepository;
        private readonly IUniversity _universityRepository;
        private readonly IMajorRepository _majorRepository;
        private readonly ApplicationDbContext _context;
        public ProfileController(UserManager<ApplicationUser> userManager, 
            ApplicationDbContext context,
            IUniversity universityRepository,
            IMajorRepository majorRepository,
            IEducationRepository educationRepository)
        {
            _userManager = userManager;
            _context = context;
            _universityRepository = universityRepository;
            _majorRepository = majorRepository;
            _educationRepository = educationRepository;
        }

        public async Task<IActionResult> Index()
        {
            var find_user = await _userManager.GetUserAsync(User);
            var educations = await _educationRepository.GetAllAsync();
            ViewBag.Educations = educations;
            if (find_user != null)
            {
                return View(find_user);
            }
            else
            {
                return NotFound();
            }

        }
        // GET: Company/Profile/Edit/id_company
        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            var find_user = await _userManager.GetUserAsync(User);
            var major = await _majorRepository.GetAllAsync();
            var university = await _universityRepository.GetAllAsync();
            ViewBag.major = new SelectList(major, "Id", "major_name");
            ViewBag.university = new SelectList(university, "Id", "university_name");

            return View(find_user);
        }
        [HttpGet]
        public async Task<IActionResult> AddEducation()
        {
         
            var majors = await _majorRepository.GetAllAsync();
            var universities = await _universityRepository.GetAllAsync();
            ViewBag.Majors = new SelectList(majors, "Id", "major_name");
            ViewBag.Universities = new SelectList(universities, "Id", "university_name");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateEducation()
        {
            var find_user = await _userManager.GetUserAsync(User);
            var major = await _majorRepository.GetAllAsync();
            var university = await _universityRepository.GetAllAsync();
            ViewBag.Major = new SelectList(major, "Id", "major_name");
            ViewBag.University = new SelectList(university, "Id", "university_name");

            return View(find_user);
        }


        // POST: Company/Home/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(string id, [Bind("fullname, date_of_birth, hard_skills, soft_skills, mobile_no, introduce_yourself, address, poisition_user")] ApplicationUser user, IFormFile image_url)
        {
            var find_user = await _userManager.GetUserAsync(User);
            if (find_user != null && id != find_user.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (user != null && find_user != null)
                        {
                            if (image_url != null && IsImageFile(image_url) && IsFileSizeValid(image_url))
                            {
                                // Lưu hình ảnh đại diện
                                find_user.image_url = await SaveImage(image_url);
                                var major = await _majorRepository.GetAllAsync();
                                var education = await _educationRepository.GetAllAsync();
                                var university = await _universityRepository.GetAllAsync();
                                ViewBag.Major = new SelectList(major, "Id", "major_name");
                                ViewBag.University = new SelectList(university, "Id", "university_name");
                            }
                            else
                            {
                                ModelState.AddModelError("ImageUrl", "Vui lòng chọn một tệp hình ảnh hợp lệ và có kích thước nhỏ hơn 5MB.");
                            }
                            find_user.fullname = user.fullname;
                            find_user.date_of_birth = user.date_of_birth;
                            find_user.mobile_no = user.mobile_no;
                            find_user.hard_skills = user.hard_skills;
                            find_user.address = user.address;
                            find_user.poisition_user = user.poisition_user;
                            find_user.soft_skills = user.soft_skills;
                            find_user.introduce_yourself = user.introduce_yourself;
                            find_user.update_at = DateTime.Now;
                            await _userManager.UpdateAsync(find_user);
                        }
                    }
                }
                catch
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(find_user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEducation(Education educationUser)
        {
            var find_user = await _userManager.GetUserAsync(User);
            if (find_user == null)
            {//Khó xảy ra vì đã chuyển hướng từ phân quyền
                return NotFound("Chưa đăng nhập");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (find_user != null)
                        {
                            educationUser.applicationUserId = find_user.Id;
                            var majors = await _majorRepository.GetAllAsync();
                            var universities = await _universityRepository.GetAllAsync();
                            ViewBag.Majors = new SelectList(majors, "Id", "major_name");
                            ViewBag.Universities = new SelectList(universities, "Id", "university_name");
                            await _educationRepository.AddAsync(educationUser);
                        }
                    }
                }
                catch
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(find_user);
        }

        private bool IsImageFile(IFormFile file)
        {
            // Kiểm tra phần mở rộng của file có phải là ảnh hay không
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(fileExtension);
        }

        //Hàm lưu ảnh
        private async Task<string> SaveImage(IFormFile image)
        {
            try
            {
                //đảm bảo tên ảnh là duy nhất khi up
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var savePath = Path.Combine("wwwroot/img", fileName); // Thay đổi đường dẫn theo cấu hình của bạn
                using (var fileStream = new FileStream(savePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
                return "/img/" + fileName;
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
