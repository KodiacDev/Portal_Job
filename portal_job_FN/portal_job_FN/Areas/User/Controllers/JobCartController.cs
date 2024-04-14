using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portal_job_FN.Models;
using portal_job_FN.Repositories;
using portal_job_FN.Session;

namespace portal_job_FN.Areas.User.Controllers
{
    [Area("User")]
    public class JobCartController : Controller
    {
        private readonly IPostJobRepository _postJobRepository;
        public JobCartController(IPostJobRepository postJobRepository)
        {
            _postJobRepository = postJobRepository;
        }
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<JobCart>("Cart") ?? new
            JobCart();
            return View(cart);

        }
        [HttpGet]
        public IActionResult CheckCartItem()
        {
            var cart = HttpContext.Session.GetObjectFromJson<JobCart>("Cart") ?? new JobCart();
            var cartItemIds = cart.Items.Select(item => item.PostJobId.ToString()).ToList();
            return Ok(cartItemIds);
        }
        [HttpPost]
        public async Task <IActionResult> AddToCart(int Id)
        {
            // Giả sử bạn có phương thức lấy thông tin sản phẩm từ productId
            var postJob = await _postJobRepository.GetByIdAsync(Id);
            var cartItem = new CartItem
            {
                PostJobId = Id,
                CompanyName = postJob.applicationUser?.company_name,
                JobName = postJob.job_name,
                ProvinceName = postJob.job_Location?.province_name,
                SalaryMin = postJob.salary_min,
                SalaryMax = postJob.salary_max,
                EmployerType = postJob.employmentType,
                ApplyDate = postJob.apply_date,
                imageUrl = postJob.applicationUser?.image_url
            };
            var cart =
            HttpContext.Session.GetObjectFromJson<JobCart>("Cart") ?? new JobCart();
            cart.AddItem(cartItem);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return NoContent();
        }
        [HttpGet]
        public IActionResult RemoveFromCart(int Id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<JobCart>("Cart");
            if (cart != null)
            {
                cart.RemoveItem(Id);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }

    }
}
