namespace portal_job_FN.Models
{
    public class Admin
    {
        //Admin
        public string? email { get; set; }
        public string? fullname { get; set; }
        public string? mobile_no { get; set; }
        public string? city { get; set; }
        public string? image_url { get; set; }
        public DateTime? create_at { get; set; }
        public DateTime? update_at { get; set; }
        public int? is_active { get; set; }
    }
}
