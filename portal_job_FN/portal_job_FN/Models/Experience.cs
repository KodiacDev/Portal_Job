namespace portal_job_FN.Models
{
    public class Experience
    {
        public int Id { get; set; }
        public string? experience_name { get; set; }
        public List<PostJob>? post_Jobs { get; set; }
    }
}
