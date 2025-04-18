namespace portal_job_FN.Dto
{
    public class PostJobDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? experience {  get; set; }
        public string? image_Url { get; set;  }
        public string? company_name { get; set; }
        public string? Description { get; set; }
        public string? detail_location { get; set; }
        public string? benefit { get; set; }
        public string? employmentType { get; set; }
        public string? required_skill { get; set; }
        public string? max_salary { get; set; }
        public DateTime? created_at { get; set; }
        public string? companyId {  get; set; }
        public List<string>? urlImages { get; set; }
    }
}
