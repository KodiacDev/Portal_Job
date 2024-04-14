namespace portal_job_FN.Models
{
    public class Company_raw
    {
        //Company

        public string? email { get; set; }
        public string? address { get; set; }
        public string? company_name { get; set; }
        //SDT ca nhan
        public string? mobile_no { get; set; }
        public string? city { get; set; }
        //SDT cong ty
        public string? contact_no { get; set; }
        public string? position_title { get; set; }
        public string? website { get; set; }
        //public DateTime? create_at { get; set; }
        //public DateTime? update_at { get; set; }
        //public int? is_active { get; set; }
        public int location_id { get; set; }
        //public List<apply_job>? apply_Jobs { get; set; }
    }
}
