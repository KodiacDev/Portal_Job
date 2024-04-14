using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using portal_job_FN.Data;
using portal_job_FN.Models;

namespace portal_job_FN.Areas.Company.Controllers
{
    [Area("Company")]
    public class apply_jobController : Controller
    {
        private readonly ApplicationDbContext _context;

        public apply_jobController(ApplicationDbContext context)
        {
            _context = context;
        }

       /* // GET: Company/apply_job
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.apply_Jobs.Include(a => a.Major).Include(a => a.applicationUser).Include(a => a.post_Job);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Company/apply_job/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apply_job = await _context.apply_Jobs
                .Include(a => a.Major)
                .Include(a => a.applicationUser)
                .Include(a => a.post_Job)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apply_job == null)
            {
                return NotFound();
            }

            return View(apply_job);
        }

        // GET: Company/apply_job/Create
        public IActionResult Create()
        {
            ViewData["MajorId"] = new SelectList(_context.majors, "Id", "Id");
            ViewData["applicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["post_JobId"] = new SelectList(_context.post_Jobs, "Id", "Id");
            return View();
        }

        // POST: Company/apply_job/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,url_cv,cover_letter,Feedback,create_at,update_at,post_JobId,application_userId,applicationUserId,MajorId")] apply_job apply_job)
        {
            if (ModelState.IsValid)
            {
                _context.Add(apply_job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MajorId"] = new SelectList(_context.majors, "Id", "Id", apply_job.MajorId);
            ViewData["applicationUserId"] = new SelectList(_context.Users, "Id", "Id", apply_job.applicationUserId);
            ViewData["post_JobId"] = new SelectList(_context.post_Jobs, "Id", "Id", apply_job.post_JobId);
            return View(apply_job);
        }

        // GET: Company/apply_job/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apply_job = await _context.apply_Jobs.FindAsync(id);
            if (apply_job == null)
            {
                return NotFound();
            }
            ViewData["MajorId"] = new SelectList(_context.majors, "Id", "Id", apply_job.MajorId);
            ViewData["applicationUserId"] = new SelectList(_context.Users, "Id", "Id", apply_job.applicationUserId);
            ViewData["post_JobId"] = new SelectList(_context.post_Jobs, "Id", "Id", apply_job.post_JobId);
            return View(apply_job);
        }

        // POST: Company/apply_job/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,url_cv,cover_letter,Feedback,create_at,update_at,post_JobId,application_userId,applicationUserId,MajorId")] apply_job apply_job)
        {
            if (id != apply_job.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(apply_job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!apply_jobExists(apply_job.Id))
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
            ViewData["MajorId"] = new SelectList(_context.majors, "Id", "Id", apply_job.MajorId);
            ViewData["applicationUserId"] = new SelectList(_context.Users, "Id", "Id", apply_job.applicationUserId);
            ViewData["post_JobId"] = new SelectList(_context.post_Jobs, "Id", "Id", apply_job.post_JobId);
            return View(apply_job);
        }

        // GET: Company/apply_job/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apply_job = await _context.apply_Jobs
                .Include(a => a.Major)
                .Include(a => a.applicationUser)
                .Include(a => a.post_Job)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apply_job == null)
            {
                return NotFound();
            }

            return View(apply_job);
        }

        // POST: Company/apply_job/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apply_job = await _context.apply_Jobs.FindAsync(id);
            if (apply_job != null)
            {
                _context.apply_Jobs.Remove(apply_job);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool apply_jobExists(int id)
        {
            return _context.apply_Jobs.Any(e => e.Id == id);
        }*/
    }
}
