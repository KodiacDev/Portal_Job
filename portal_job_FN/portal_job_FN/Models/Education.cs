namespace portal_job_FN.Models
{
    public class Education
    {
        public int Id { get; set; }
        public float? gpa {  get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set;}
        public string? applicationUserId { get; set; }
        public int? universityId {  get; set; }
        public int? MajorId { get; set; }
        public ApplicationUser? applicationUser { get; set; }
        public University? university { get; set; }
        public Major? Major { get; set; }
    }
}
