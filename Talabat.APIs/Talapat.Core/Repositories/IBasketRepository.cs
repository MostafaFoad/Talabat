using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talapat.Core.Entities;

namespace Talapat.Core.Repositories
{
	public interface IBasketRepository
	{
		Task<CustomerBasket?> GetBasketAsync(string BasketId);
		Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket Basket);
		Task<bool> DeleteBasketAsync(string BasketId);
		
	}
}
