using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portal_job_FN.Dto;
using portal_job_FN.Models;
using portal_job_FN.Repositories;

namespace portal_job_FN.Controllers
{
    [Route("api/Jobs")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly IPostJobRepository _postJob;

        public APIController(IPostJobRepository postJob)
        {
            _postJob = postJob;
        }

        // GET: api/Jobs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostJobDto>>> GetJobs()
        {
            var jobs = await _postJob.GetAllJobAPIAsync();
            if (jobs == null || !jobs.Any())
            {
                return NotFound();
            }
            return Ok(jobs);
        }


        // GET: api/Jobs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostJob>> GetJob(int id)
        {
            var job = await _postJob.GetByIdAsync(id);

            if (job == null)
            {
                return BadRequest();
            }

            return job;
        }
    
    }
}
