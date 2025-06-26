using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace BOGOMATCH.Controllers.V1.AccountController
{

    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly StripeClient _stripeClient;
        private readonly PaymentIntentService _paymentIntentService;

        public PaymentController(StripeClient stripeClient, PaymentIntentService paymentIntentService)
        {
            _stripeClient = stripeClient;
            _paymentIntentService = paymentIntentService;
        }

        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent(string PaymentMethodName, long Amount)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = Amount,
                Currency = "usd",
                PaymentMethod = PaymentMethodName,
                ConfirmationMethod = "automatic",
                Confirm = true,
                ReturnUrl = "https://localhost:7066/swagger/payment-sucsess",
            };

            try
            {
                var service = new PaymentIntentService(_stripeClient);
                var intent = await service.CreateAsync(options);

                return Ok(new { clientSecret = intent.ClientSecret, successMessage = "Payment successful!" });
            }
            catch (StripeException ex)
            {
                return BadRequest(new { errorMessage = ex.Message, failureMessage = "Payment failed. Please try again." });
            }
        }

        [HttpGet("payment-return")]
        public async Task<IActionResult> PaymentReturn(string paymentIntentClientSecret)
        {
            var service = new PaymentIntentService();
            PaymentIntent paymentIntent = null;

            try
            {
                paymentIntent = await service.GetAsync(paymentIntentClientSecret);

                if (paymentIntent.Status == "failed" || paymentIntent.Status == "requires_payment_method")
                {
                    return RedirectToAction("PaymentFailure");
                }

                return RedirectToAction("PaymentSuccess");
            }
            catch (StripeException ex)
            {
                return RedirectToAction("PaymentFailure", new { message = "An error occurred while processing the payment." });
            }
        }

    }
}
