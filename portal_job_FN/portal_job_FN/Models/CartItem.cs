using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace portal_job_FN.Models
{
    public class CartItem
    {
        public int PostJobId { get; set; }
        public string? JobName { get; set; }
        public string? CompanyName {  get; set; }
        public string? ProvinceName { get; set; }
        public int? SalaryMin {  get; set; }
        public int? SalaryMax {  get; set; }
        public string? EmployerType {  get; set; }
        public DateTime? ApplyDate {  get; set; }
        public string? imageUrl { get; set; }

    }
}
