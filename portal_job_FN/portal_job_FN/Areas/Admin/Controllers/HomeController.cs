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

namespace portal_job_FN.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class HomeController : Controller
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IExperienceRepository _experience;
        private readonly IMajorRepository _major;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IPostJobRepository _post_job;
        private readonly IApplyJobRepository _apply_job;
        private readonly IUserRepository _userRepository;
        private readonly IEducationRepository _educationRepository;

        public HomeController(ApplicationDbContext context,
            ILocationRepository locationRepository,
            IExperienceRepository experience,
            IMajorRepository major,
            IPostJobRepository post_job,
            UserManager<ApplicationUser> userManager,
            IApplyJobRepository apply_job,
            IUserRepository userRepository,
            IEducationRepository educationRepository)
        {
            _context = context;
            _locationRepository = locationRepository;
            _experience = experience;
            _major = major;
            _post_job = post_job;
            _userManager = userManager;
            _apply_job = apply_job;
            _userRepository = userRepository;
            _educationRepository = educationRepository;
        }

        // GET: Company/Home
        public async Task<IActionResult> Index()
        {
            var find_admin = await _userManager.GetUserAsync(User);
            if (find_admin == null)
            {
                return NotFound("Chưa đăng nhập");
            }
            return View(find_admin);
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

        public async Task<IActionResult> ListUser()
        {
            var listUser = await _userRepository.GetAllAsync();
            return View(listUser);
        }

        public async Task<IActionResult> ListCompany()
        {
            var listUser = await _userRepository.GetAllCompanyAsync();
            return View(listUser);
        }

        [HttpGet]
        public async Task<IActionResult> deleteSoftCompany(string id)
        {
            var listUser = await _userRepository.GetByIdAsync(id);
            if (listUser == null)
            {
                return NotFound();
            }
            else
            {
                listUser.is_active = 0;
                await _userManager.UpdateAsync(listUser);
            }
            return RedirectToAction(nameof(ListCompany));
        }
        [HttpGet]
        public async Task<IActionResult> activeSoftCompany(string id)
        {
            var listUser = await _userRepository.GetByIdAsync(id);
            if (listUser == null)
            {
                return NotFound();
            }
            else
            {
                listUser.is_active = 1;
                await _userManager.UpdateAsync(listUser);
            }
            return RedirectToAction(nameof(ListCompany));
        }


        [HttpGet]
        public async Task<IActionResult> deleteSoft(string id)
        {
            var listUser = await _userRepository.GetByIdAsync(id);
            if (listUser == null)
            {
                return NotFound();
            }
            else
            {
                listUser.is_active = 0;
                await _userManager.UpdateAsync(listUser);
            }
            return RedirectToAction(nameof(ListUser));
        }
        [HttpGet]
        public async Task<IActionResult> activeSoft(string id)
        {
            var listUser = await _userRepository.GetByIdAsync(id);
            if (listUser == null)
            {
                return NotFound();
            }
            else
            {
                listUser.is_active = 1;
                await _userManager.UpdateAsync(listUser);
            }
            return RedirectToAction(nameof(ListUser));
        }


        public async Task<IActionResult> DetailsUser(string id)
        {
            var find_user = await _userRepository.GetByIdAsync(id);
            if (find_user != null)
            {
                var educations = await _educationRepository.GetByIdUserAsync(find_user.Id);
                ViewBag.Educations = educations;
                return View(find_user);
            }
            else
            {
                return NotFound();
            }
        }
        public async Task<IActionResult> DetailsCompany(string id)
        {
            var find_user = await _userRepository.GetByIdAsync(id);
            if (find_user != null)
            {
                return View(find_user);
            }
            else
            {
                return NotFound();
            }
        }


        /*  // GET: Company/Home/Edit/5
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
          }*/



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
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
