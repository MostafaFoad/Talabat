using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talapat.Core.Entities;
using Talapat.Core.Entities.Order_Aggregation;
using Talapat.Core.Repositories;
using Talapat.Core.Services;

namespace Talapat.Services
{
	public class OrderServices : IOrderServices
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IPaymetService _paymetService;

		public OrderServices(IBasketRepository basketRepository, IUnitOfWork unitOfWork,IPaymetService paymetService)
		{
			_basketRepository = basketRepository;
			_unitOfWork = unitOfWork;
			_paymetService = paymetService;
		}
		public async Task<Order?> CreateOrderAsync(string buyeremail, string BasketId, int deliveryMethodId, Address shipingaddress)
		{

			var basket = await _basketRepository.GetBasketAsync(BasketId);
			if (basket is null) return null;
			var Orderitems = new List<OrderItem>();
			
				foreach (var item in basket.Items)
				{
					var productrepoobj = _unitOfWork.CreateRepo<Product>();
					if (productrepoobj is not null)
					{
						var product = await productrepoobj.GetByIdAsync(item.Id);
						var productitem = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
						var orderitem = new OrderItem(productitem, product.Price, item.Quantity);
						Orderitems.Add(orderitem);
					}
				}
			
			var subtotla = Orderitems.Sum(I => I.Price * I.Quantity);
			var DeliveryMethodrepo = _unitOfWork.CreateRepo<DeliveryMethod>();
			DeliveryMethod delmetho=new DeliveryMethod();
			if (DeliveryMethodrepo is not null)
			 delmetho = await DeliveryMethodrepo.GetByIdAsync(deliveryMethodId);

			var spec = new OrderWithPaymentIdSpecification(basket.PaymentIntentId);
			var Exist_order = await _unitOfWork.CreateRepo<Order>().GetByIdWithSpecAsync(spec);
			if (Exist_order is not null)
			{
				_unitOfWork.CreateRepo<Order>().Delete(Exist_order);
			}
				await _paymetService.CreateOrUpdatePaymentIntent(BasketId);// if create payment intent for basket and open new tap and add item to baske create order for this basket

			var order = new Order(buyeremail, shipingaddress, delmetho, Orderitems, subtotla,basket.PaymentIntentId);
			var orderrepo = _unitOfWork.CreateRepo<Order>();
			if(orderrepo is not null)
			await orderrepo.AddAsync(order);

			var res = await _unitOfWork.Complete();

			return res > 0 ? order : null;
		}

		public async Task<Order?> GetOrderByIdForUserAsync(string Email, int orderId)
		{
			var spec = new OrderSpecifications(Email, orderId);
			var order= await _unitOfWork.CreateRepo<Order>().GetByIdWithSpecAsync(spec);
			if (order is null) return null;
			return order;
		}

		public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string Email)
		{
			var spec = new OrderSpecifications(Email);
			return await _unitOfWork.CreateRepo<Order>().GetAllWithSpecAsync(spec);

		}
		public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
		{
			return await _unitOfWork.CreateRepo<DeliveryMethod>().GetAllAsync();

		}
	}
}
