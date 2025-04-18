using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using portal_job_FN.Models;
using portal_job_FN.Repositories;
using portal_job_FN.Services.Vnpay;

namespace portal_job_FN.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CheckoutController : Controller
    {
        private readonly IVnPayService _vnPayService;
        private readonly IVnPayRepository _vnPayRepository;
        private readonly UserManager<ApplicationUser> _userManager;
      
        public CheckoutController(IVnPayService vnPayService, 
            UserManager<ApplicationUser> userManager,
            IVnPayRepository vnPayRepository) 
        { 
            _vnPayService = vnPayService;
            _userManager = userManager;
            _vnPayRepository = vnPayRepository;
        }


        [HttpGet]
        public async Task<IActionResult> PaymentCallbackVnpay()
        {
            var find_company = await _userManager.GetUserAsync(User);
            var response = _vnPayService.PaymentExecute(Request.Query);

            // Kiểm tra nếu giao dịch đã tồn tại trong database
            var existingTransaction = await _vnPayRepository.GetTransactionByIdAsync(response.TransactionId);
            if (existingTransaction == null && response.VnPayResponseCode == "00")
            {
                var postCountIncrement = 0;
                // Xác định số bài viết được cộng thêm dựa trên số tiền
                if (response.Amount >= 199000)
                {
                    postCountIncrement = 35;
                }
                else if (response.Amount >= 99000)
                {
                    postCountIncrement = 15;
                }
                else if (response.Amount >= 49000)
                {
                    postCountIncrement = 5;
                }
                // Tăng số bài viết của công ty
                find_company.PostCount += postCountIncrement;
                var newVnpay = new VnpayModel
                {
                    OrderDescription = response.OrderDescription,
                    TransactionId = response.TransactionId,
                    OrderId = response.OrderId,
                    PaymentMethod = response.PaymentMethod,
                    PostCount = find_company.PostCount,
                    PaymentId = response.PaymentId,
                    Amount = response.Amount,
                    create_at = DateTime.Now,
                    applicationUserId = find_company.Id
                };
                // Lưu giao dịch và cập nhật dữ liệu người dùng
                await _vnPayRepository.AddBillAsync(newVnpay);
                await _userManager.UpdateAsync(find_company);

                return View(newVnpay);
            }
            else
            {
                // Xử lý nếu giao dịch không thành công
                return View();
            }
        }

    }
}
