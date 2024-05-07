using Microsoft.VisualBasic;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talapat.Core.Entities;
using Talapat.Core.Repositories;

namespace Talapat.Repository
{
	public class BasketRepository : IBasketRepository
	{
		private readonly IDatabase _database;

		public BasketRepository(IConnectionMultiplexer redis)
		{
			_database = redis.GetDatabase();
		}
		public async Task<bool> DeleteBasketAsync(string BasketId)
		{
			return await _database.KeyDeleteAsync(BasketId);
		}

		public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
		{
			var basket= await _database.StringGetAsync(BasketId);
			if (basket.IsNull) return null;
			 var json=  JsonSerializer.Deserialize<CustomerBasket>(basket);
			return json;
		}

		public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket Basket)
		{
			bool createdorupdated=await _database.StringSetAsync(Basket.Id, JsonSerializer.Serialize(Basket), TimeSpan.FromDays(1));
			if (!createdorupdated) return null;
			return await GetBasketAsync(Basket.Id);
			
		}
	}
}
