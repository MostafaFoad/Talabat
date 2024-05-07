using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talapat.Core.Entities;
using Talapat.Core.Entities.Order_Aggregation;
using Talapat.Core.Services;

namespace Talabat.APIs.Controllers
{
	[Authorize]
	public class PaymentsController : BaseApiController
	{
		private readonly IPaymetService _paymetService;
		private readonly ILogger<PaymentsController> _logger;
		const string endpointSecret = "whsec_172916596938f78b9bb27ac21264ede4b4ee11c651515e58177392a79561f0a7";


		public PaymentsController(IPaymetService paymetService,ILogger<PaymentsController> logger)
		{
			_paymetService = paymetService;
			_logger = logger;
		}
		[HttpPost("{basketId}")]
		public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
		{
			var basket=await _paymetService.CreateOrUpdatePaymentIntent(basketId);
			if (basket is null) return BadRequest(new ApiResponse(400,"A problem with our basket"));
			return Ok(basket);
		}
		[AllowAnonymous]
		[HttpPost("webhook")]
		public async Task<IActionResult> StripeWebhook()
		{
			var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
			Order order;
				var stripeEvent = EventUtility.ConstructEvent(json,
					Request.Headers["Stripe-Signature"], endpointSecret);

				// Handle the event
				var PaymentIntent = (PaymentIntent)stripeEvent.Data.Object;
				
				switch (stripeEvent.Type)
				{
					case Events.PaymentIntentSucceeded:
						order=await _paymetService.UpdatePaymentIntentToSucceedOrFaild(PaymentIntent.Id,true);
						_logger.LogInformation("Payment Succeed", PaymentIntent.Id);
						break;
					case Events.PaymentIntentPaymentFailed:
						order=await _paymetService.UpdatePaymentIntentToSucceedOrFaild(PaymentIntent.Id,false);
						_logger.LogInformation("Payment Faild", PaymentIntent.Id);
						break;
				}

			

				return Ok();
			
			
		}

	}
}
