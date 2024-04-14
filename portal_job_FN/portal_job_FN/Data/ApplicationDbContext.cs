using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using portal_job_FN.Models;

namespace portal_job_FN.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplyJob> apply_Jobs { get; set; }
        public DbSet<UserImage> company_image { get; set; }
        public DbSet<Education> educations { get; set; }
        public DbSet<JobLocation> job_location { get; set; }
        public DbSet<Experience> experience { get; set; }
        public DbSet<Major> majors { get; set; }
        public DbSet<PostJob> post_Jobs { get; set; }
        public DbSet<PostJobImage> post_Jobs_images { get; set; }
        public DbSet<University> universities { get; set; }

    }
}
