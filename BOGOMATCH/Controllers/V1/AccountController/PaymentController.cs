using BOGOMATCH_DOMAIN.MODELS.NewFolder;
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
        public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentRequest request)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = request.Amount,
                Currency = "usd",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            };

            var intent = await _paymentIntentService.CreateAsync(options);
            return Ok(new { clientSecret = intent.ClientSecret });
        }



    }
}
