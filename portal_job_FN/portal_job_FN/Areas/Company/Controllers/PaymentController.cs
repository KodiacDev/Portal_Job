using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using portal_job_FN.Models;
using portal_job_FN.Models.Vnpay;
using portal_job_FN.Repositories;
using portal_job_FN.Services.Vnpay;

namespace portal_job_FN.Areas.Company.Controllers
{
    [Area("Company")]
    [Authorize(Roles = SD.Role_Company)]
    public class PaymentController : Controller
    {
        private readonly IVnPayService _vnPayService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVnPayRepository _vnPayRepository;
        public PaymentController(IVnPayService vnPayService, UserManager<ApplicationUser> userManager, IVnPayRepository vnPayRepository)
        {
            _vnPayService = vnPayService;
            _userManager = userManager;
            _vnPayRepository = vnPayRepository;
        }
        [HttpPost]
        public  IActionResult CreatePaymentUrlVnpay(PaymentInformationModel model)
        {
            var url =  _vnPayService.CreatePaymentUrl(model, HttpContext);
            return Redirect(url);
        }
        [HttpGet]
        public async Task<IActionResult> ListHistoryPayment()
        {
            var find_company = await _userManager.GetUserAsync(User);
            ViewBag.user = find_company;
            var listHistoryPay = await _vnPayRepository.GetAllHistoryPayByIdCompany(find_company.Id);
            if (find_company != null && listHistoryPay != null)
            {
                return View(listHistoryPay);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        public async Task<IActionResult> DetailPayment(int id)
        {
            var hisToryPay = await _vnPayRepository.GetHistoryById(id);
            if (hisToryPay != null)
            {
                return View(hisToryPay);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Purchase()
        {
            var find_company =await _userManager.GetUserAsync(User);
            if (find_company != null)
            {
                return View(find_company);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
