using Microsoft.EntityFrameworkCore;
using portal_job_FN.Data;
using portal_job_FN.Models;

namespace portal_job_FN.Repositories
{
    public class EFVnPayRepository : IVnPayRepository
    {
        private readonly ApplicationDbContext _context;
        public EFVnPayRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddBillAsync(VnpayModel vnpay)
        {
            _context.vnpays.Add(vnpay);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<VnpayModel>> GetAllHistoryPayByAdmin()
        {
            var applicationDbContext = await _context.vnpays
               .Include(b => b.applicationUser)
               .OrderByDescending(b => b.create_at)
               .ToListAsync();
            return applicationDbContext;
        }

        public async Task<IEnumerable<VnpayModel>> GetAllHistoryPayByIdCompany(string idCompany)
        {
            var applicationDbContext = await _context.vnpays
              .Include(b => b.applicationUser)
              .OrderByDescending(b => b.create_at)
              .Where(b => b.applicationUser.Id == idCompany)
              .ToListAsync();
            return applicationDbContext;
        }

        public async Task<VnpayModel> GetHistoryById(int idVnpay)
        {
            return await _context.vnpays.FindAsync(idVnpay);
        }
        public async Task<ApplyJob> GetByIdAsync(int id)
        {
            var applicationDbContext = await _context.apply_Jobs
                .Include(b => b.post_Job)
                .Include(b => b.applicationUser)
                .ToListAsync();
            return await _context.apply_Jobs.FindAsync(id);
        }
        public async Task<VnpayModel> GetTransactionByIdAsync(string transactionId)
        {
            return await _context.vnpays.FirstOrDefaultAsync(t => t.TransactionId == transactionId);
        }

     
        public async Task<decimal> TotalMoneyByCompanyId(string idCompany)
        {
            // Tìm tất cả các khoản tiền của công ty với idCompany
            var totalMoney = await _context.vnpays
                .Where(v => v.applicationUserId == idCompany)
                .SumAsync(v => v.Amount);

            // Trả về tổng số tiền
            return (decimal)totalMoney;
        }

        public async Task<decimal> TotalMoneyByAdmin()
        {
            // Tìm tất cả các khoản tiền của các công ty 
            var totalMoney = await _context.vnpays
                .SumAsync(v => v.Amount);
            // Trả về tổng số tiền
            return (decimal)totalMoney;
        }
    }
}
