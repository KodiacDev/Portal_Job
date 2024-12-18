namespace portal_job_FN.Models
{
    public class VnpayModel
    {
        public int Id { get; set; }
        public string? OrderDescription { get; set; }
        public string? TransactionId { get; set; }
        public string? OrderId { get; set; }
        public string? PaymentMethod { get; set; }
        public int? PostCount { get; set; }
        public string? PaymentId { get; set; }
        public double? Amount { get; set; }
        public DateTime? create_at { get; set; }
        public string? applicationUserId { get; set; }
        public ApplicationUser? applicationUser { get; set; }
    }
}
