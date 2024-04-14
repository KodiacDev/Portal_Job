using Microsoft.AspNetCore.Identity;
using System.Data;

namespace portal_job_FN.Models
{
    public class ApplicationUser : IdentityUser
    {
        //Gộp User, Admin, Company(Nhà tuyển dụng vào 1 object duy nhất ApplicationUser)
        //User
        //public string? email { get; set; }
        public string? mobile_no { get; set; }
        public string? city { get; set; }
        public string? fullname { get; set; }
        public string? date_of_birth { get; set; }
        public string? image_url { get; set; }
        public string? hard_skills { get; set; }
        public string? soft_skills { get; set; }
        public string? introduce_yourself { get; set; }
        public string? poisition_user {  get; set; }
        public DateTime? create_at { get; set; }
        public DateTime? update_at { get; set; }
        public int? is_active { get; set; }
        public List<ApplyJob>? apply_Jobs { get; set; }
        public List<Education>? educations { get; set; }

        //Admin
        //public string? email { get; set; }
        //public string? fullname { get; set; }
        //public string? mobile_no { get; set; }
        //public string? city { get; set; }
        //public string? image_url { get; set; }
        //public DateTime? create_at { get; set; }
        //public DateTime? update_at { get; set; }
        //public int? is_active { get; set; }

        //Company
        //public string? email { get; set; }
        public string? address { get; set; }
        public string? introduce_company {  get; set; }
        public string? company_name { get; set; }
        //public string? mobile_no { get; set; }
        //public string? city { get; set; }
        public string? contact_no { get; set; }
        public string? position_title { get; set; }
        public string? website { get; set; }
        //public DateTime? create_at { get; set; }
        //public DateTime? update_at { get; set; }
        //public int? is_active { get; set; }
        public int location_id { get; set; }
        //public List<apply_job>? apply_Jobs { get; set; }
        public List<PostJob>? post_Jobs { get; set; }
        public List<UserImage>? company_Images { get; set; }
    }
}
