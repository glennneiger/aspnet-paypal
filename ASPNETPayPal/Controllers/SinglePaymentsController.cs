using ASPNETPayPal.Services;
using System.Web.Mvc;

namespace ASPNETPayPal.Controllers
{
    public class SinglePaymentsController : Controller
    {
        // GET: SinglePayments
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePayment()
        {
            var baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
            var payment = PayPalPaymentService.CreatePayment(baseUrl, "sale");

            return Redirect(payment.GetApprovalUrl());
        }

        public ActionResult PaymentCancelled()
        {
            return RedirectToAction("Error");
        }

        public ActionResult PaymentSuccessful(string paymentId, string token, string payerId)
        {
            var payment = PayPalPaymentService.ExecutePayment(paymentId, payerId);

            return View();
        }

        public ActionResult PaymentAuthorized()
        {
            return View();
        }
    }
}