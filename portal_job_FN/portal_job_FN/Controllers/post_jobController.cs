using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using portal_job_FN.Data;
using portal_job_FN.Models;

namespace portal_job_FN.Controllers
{
    public class post_jobController : Controller
    {
        private readonly ApplicationDbContext _context;

        public post_jobController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: post_job
        public async Task<IActionResult> Index()
        {
            return View(await _context.post_Jobs.ToListAsync());
        }

        // GET: post_job/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post_job = await _context.post_Jobs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post_job == null)
            {
                return NotFound();
            }

            return View(post_job);
        }

        // GET: post_job/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: post_job/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,job_name,job_description,required_skill,benefit,employmentType,salary_min,salary_max,detail_location,create_at,update_at,apply_date,is_active,location_id,experience_id,application_user,major_id")] PostJob post_job)
        {
            if (ModelState.IsValid)
            {
                _context.Add(post_job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post_job);
        }

        // GET: post_job/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post_job = await _context.post_Jobs.FindAsync(id);
            if (post_job == null)
            {
                return NotFound();
            }
            return View(post_job);
        }

        // POST: post_job/Edit/5
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
                    _context.Update(post_job);
                    await _context.SaveChangesAsync();
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

        // GET: post_job/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post_job = await _context.post_Jobs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post_job == null)
            {
                return NotFound();
            }

            return View(post_job);
        }

        // POST: post_job/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post_job = await _context.post_Jobs.FindAsync(id);
            if (post_job != null)
            {
                _context.post_Jobs.Remove(post_job);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool post_jobExists(int id)
        {
            return _context.post_Jobs.Any(e => e.Id == id);
        }
    }
}
