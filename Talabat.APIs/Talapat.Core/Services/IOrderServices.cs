using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talapat.Core.Entities.Order_Aggregation;

namespace Talapat.Core.Services
{
	public interface IOrderServices
	{
		public Task<Order?> CreateOrderAsync(string buyeremail, string BasketId, int deliveryMethoId, Address shipingaddress);
		public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string Email);
		public Task<Order?> GetOrderByIdForUserAsync(string Email, int orderId);
		public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync();
	}
}
