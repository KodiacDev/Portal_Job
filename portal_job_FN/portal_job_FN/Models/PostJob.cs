namespace portal_job_FN.Models
{
    public class PostJob
    {
        public int Id { get; set; }
        public string? job_name { get; set; }
        public string? job_description { get; set; }
        public string? required_skill { get; set; }
        public string? benefit {  get; set; }
        public string? employmentType {  get; set; }
        public int? salary_min {  get; set; }
        public int? salary_max { get; set; }
        public string? detail_location {  get; set; }
        public DateTime? create_at { get; set; }
        public DateTime? update_at { get; set; }
        public DateTime? apply_date { get; set; }
        public int? is_active { get; set; }
        public int? job_LocationId { get; set; }
        public int? experienceId { get; set; }
        //Tìm ai đăng bài
        public string? applicationUserId { get; set; }
        public int? majorId {  get; set; }
        public Major? major { get; set; }
        public ApplicationUser? applicationUser {  get; set; }
        public Experience? experience { get; set; }
        public JobLocation? job_Location { get; set; }
        public List<PostJobImage>? post_Job_Images { get; set; }
    }
}
