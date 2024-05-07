using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talapat.Core.Entities;
using Talapat.Core.Entities.Order_Aggregation;
using Talapat.Core.Repositories;
using Talapat.Core.Services;
using Product = Talapat.Core.Entities.Product;

namespace Talapat.Services
{
	public class PaymentService : IPaymetService
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IConfiguration _configuration;

		public PaymentService(IBasketRepository basketRepository,IUnitOfWork unitOfWork,IConfiguration configuration)
		{
			_basketRepository = basketRepository;
			_unitOfWork = unitOfWork;
			_configuration = configuration;
		}
		public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId)
		{
			StripeConfiguration.ApiKey = _configuration["StripeSeetings:SecretKey"];
			var basket=await _basketRepository.GetBasketAsync(BasketId);
			if (basket is null) return null;
			decimal deliverycost = 0m;
			if (basket.DeliveryMethodId.HasValue)
			{
				var deliverymethod = await _unitOfWork.CreateRepo<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
			    basket.ShippingCost=deliverymethod.Cost;
				deliverycost = deliverymethod.Cost;
			}

			if (basket?.Items?.Count > 0)
			{
				foreach (var item in basket.Items)
				{
					var product = await _unitOfWork.CreateRepo<Product>().GetByIdAsync(item.Id);
					if (item.Price != product.Price)
						item.Price = product.Price;
				}
			}

			var service = new PaymentIntentService();
			PaymentIntent paymentIntent;
			if(string.IsNullOrEmpty( basket.PaymentIntentId))
			{

				var option = new PaymentIntentCreateOptions()
				{
					Amount = (long)(deliverycost * 100) + (long)basket.Items.Sum(i => i.Quantity * i.Price * 100),
					Currency = "usd",
					PaymentMethodTypes = new List<string>() {"card"}

			     };

				paymentIntent=await service.CreateAsync(option);
				basket.PaymentIntentId = paymentIntent.Id;
				basket.ClientSecret = paymentIntent.ClientSecret;
			}
			else
			{
				var option = new PaymentIntentUpdateOptions()
				{
					Amount = (long)(deliverycost * 100) + (long)basket.Items.Sum(i => i.Quantity * i.Price * 100)
				};
				paymentIntent = await service.UpdateAsync(basket.PaymentIntentId,option);

			}
			await _basketRepository.UpdateBasketAsync(basket);
			return basket;
		}

		public async Task<Order> UpdatePaymentIntentToSucceedOrFaild(string paymentid, bool State)
		{
			var spec = new OrderWithPaymentIdSpecification(paymentid);
			var Exist_order = await _unitOfWork.CreateRepo<Order>().GetByIdWithSpecAsync(spec);
			Exist_order.Status = State ? OrderStatus.PaymentReceived : OrderStatus.PaymentFailed;
			_unitOfWork.CreateRepo<Order>().Update(Exist_order);
			await _unitOfWork.Complete();
			return Exist_order;
		}
	}
	}

