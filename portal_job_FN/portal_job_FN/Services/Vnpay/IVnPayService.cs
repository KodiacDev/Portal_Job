using portal_job_FN.Models.Vnpay;

namespace portal_job_FN.Services.Vnpay
{
    public interface IVnPayService
    {
        // tạo ra URL thanh toán tại VnPay
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        //thực thi kiểm tra thông tin giao dịch và sẽ lưu lại khi thành công
        PaymentResponseModel PaymentExecute(IQueryCollection collections);

    }
}
