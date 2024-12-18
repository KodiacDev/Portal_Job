namespace portal_job_FN.Models
{
    public class JobLocation
    {
        public int Id { get; set; }
        public string? province_name { get; set; }
        public List<PostJob>? post_Jobs { get; set; } 
        public List<ApplicationUser>? applicationUsers { get; set; }
    }
}
