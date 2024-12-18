namespace portal_job_FN.Models
{
    public class University
    {
        public int Id { get; set; }
        public string? university_name {  get; set; }
        public List<Education>? educations { get; set; }
    }
}
