namespace portal_job_FN.Models
{
    public class ApplyJob
    {
        public int Id { get; set; }
        public string? url_cv { get; set; }
        public string? cover_letter { get; set; }
        public string? Feedback { get; set; }
        public string? Email {  get; set; }
        public string? FullName {  get; set; }
        public string? imageCompany {  get; set; }
        public string? companyName {  get; set; }
        public string? emailCompany {  get; set; }
        public string? contact_noCompany {  get; set; }
        public DateTime create_at { get; set; }
        public DateTime update_at { get; set; }
        public int post_JobId { get; set; }
        //Id của company
        public string? application_userId { get; set; }
        //Id của người tìm việc
        public string? applicationUserId {  get; set; }
        public PostJob? post_Job { get; set; }
        public ApplicationUser? applicationUser { get; set; }
    }
}
