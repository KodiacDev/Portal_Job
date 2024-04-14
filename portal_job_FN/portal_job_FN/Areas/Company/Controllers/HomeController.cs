using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
    public class HomeController : Controller
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IExperienceRepository _experience;
        private readonly IMajorRepository _major;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IPostJobRepository _post_job;
 
        public HomeController(ApplicationDbContext context,
            ILocationRepository locationRepository,
            IExperienceRepository experience,
            IMajorRepository major,
            IPostJobRepository post_job,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _locationRepository = locationRepository;
            _experience = experience;
            _major = major;
            _post_job = post_job;
            _userManager = userManager;
        }

        // GET: Company/Home
        public async Task<IActionResult> Index()
        {
            return View();
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
        public async Task<IActionResult> Create([Bind("Id,job_name,job_description,required_skill,benefit,employmentType,salary_min,salary_max,detail_location,apply_date,job_LocationId,experienceId,majorId")] PostJob post_job)
        {
            if (ModelState.IsValid)
            {
                //Lấy ra id công ty đăng baì
                string publisherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //Gán vào post_job
                post_job.applicationUserId = publisherId;
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

        // POST: Company/Home/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,job_name,job_description,required_skill,benefit,employmentType,salary_min,salary_max,detail_location,apply_date,job_LocationId,experienceId,majorId")] PostJob post_job)
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
                        post_job.create_at = find_company.create_at;
                        //Lưu thông tin cty đăng bài
                        post_job.applicationUserId = find_company.Id;
                        post_job.create_at = find_company.create_at;
                        post_job.update_at = DateTime.Now;
                        post_job.is_active = 1;
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

        private bool post_jobExists(int id)
        {
            return _context.post_Jobs.Any(e => e.Id == id);
        }
    }
}
