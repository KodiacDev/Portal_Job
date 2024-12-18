using Microsoft.EntityFrameworkCore;
using portal_job_FN.Models;

namespace portal_job_FN.Repositories
{
    public interface IVnPayRepository
    {
         Task AddBillAsync(VnpayModel vnpay);
         Task<decimal> TotalMoneyByAdmin();
         Task<decimal> TotalMoneyByCompanyId(string idCompany);
         Task<IEnumerable<VnpayModel>> GetAllHistoryPayByAdmin();

         Task<IEnumerable<VnpayModel>> GetAllHistoryPayByIdCompany(string idCompany);

         Task<VnpayModel> GetHistoryById(int idVnpay);
         Task<VnpayModel> GetTransactionByIdAsync(string transactionId);

    }
}
