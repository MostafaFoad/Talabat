using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talapat.Core.Entities;
using Talapat.Core.Entities.Order_Aggregation;

namespace Talapat.Core.Services
{
	public interface IPaymetService
	{
		public Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId);
		public Task<Order> UpdatePaymentIntentToSucceedOrFaild(string paymentid, bool State);
	}
}
