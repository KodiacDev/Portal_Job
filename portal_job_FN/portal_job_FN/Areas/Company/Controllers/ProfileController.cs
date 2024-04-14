using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using portal_job_FN.Data;
using portal_job_FN.Models;
using portal_job_FN.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace portal_job_FN.Areas.Company.Controllers
{
    [Area("Company")]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILocationRepository _locationRepository;
        private readonly ApplicationDbContext _context;
        public ProfileController(UserManager<ApplicationUser> userManager, ILocationRepository locationRepository, ApplicationDbContext context)
        {
            _userManager = userManager;
            _locationRepository = locationRepository; 
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var find_company = await _userManager.GetUserAsync(User);
            if (find_company != null)
            {
                return View(find_company);
            }
            else
            {
                return NotFound();
            }
               
        }
        // GET: Company/Profile/Edit/id_company
        public async Task<IActionResult> UpdateProfile()
        {
            var find_company = await _userManager.GetUserAsync(User);
            var locations = await _locationRepository.GetAllAsync();
            ViewBag.Locations = new SelectList(locations, "Id", "province_name");
            return View(find_company);
        }

            // POST: Company/Home/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(string id, [Bind("company_name, address, introduce_company, position_title, mobile_no, contact_no, Email, website")] ApplicationUser company, IFormFile image_url)
        {
            var find_company = await _userManager.GetUserAsync(User);
            if (find_company != null && id != find_company.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (company != null && find_company != null)
                        {
                            if (image_url != null && IsImageFile(image_url) && IsFileSizeValid(image_url))
                            {
                                // Lưu hình ảnh đại diện
                                find_company.image_url = await SaveImage(image_url);
                                var locations = await _locationRepository.GetAllAsync();
                                ViewBag.Locations = new SelectList(locations, "Id", "province_name");
                            }
                            else
                            {
                                ModelState.AddModelError("ImageUrl", "Vui lòng chọn một tệp hình ảnh hợp lệ và có kích thước nhỏ hơn 5MB.");
                            }
                            find_company.company_name = company.company_name;
                            find_company.address = company.address;
                            find_company.position_title = company.position_title;
                            find_company.mobile_no = company.mobile_no;
                            find_company.contact_no = company.contact_no;
                            find_company.Email = company.Email;
                            find_company.is_active = 1;
                            find_company.update_at = DateTime.Now;
                            find_company.website = company.website;
                            find_company.introduce_company = company.introduce_company;
                            await _userManager.UpdateAsync(find_company);
                        }
                    }
                }
                catch
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(find_company);
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
