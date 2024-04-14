namespace portal_job_FN.Models
{
    public class PostJobImage
    {
        public int Id { get; set; }
        public string? post_image_url { get; set; }
        public int post_JobId { get; set; }
        public PostJob? post_Job { get; set; }
    }
}
