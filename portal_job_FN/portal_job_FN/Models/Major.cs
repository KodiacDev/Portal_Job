namespace portal_job_FN.Models
{
    public class Major
    {
        public int Id { get; set; }
        public string? major_name { get; set; }
        public List<Education>? educations { get; set; }
        public List<PostJob>? post_jobs { get; set; }
    }
}
