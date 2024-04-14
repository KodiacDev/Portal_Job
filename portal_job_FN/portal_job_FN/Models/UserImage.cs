namespace portal_job_FN.Models
{
    public class UserImage
    {
        public int Id { get; set; }
        public string? applicationUser_image_url {  get; set; }
        public string? applicationUserId { get; set; }
        public ApplicationUser? applicationUser { get; set; }
    }
}
