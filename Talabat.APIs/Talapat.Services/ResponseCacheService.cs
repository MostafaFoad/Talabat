using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talapat.Core.Services;

namespace Talapat.Services
{
	public class ResponseCacheService : IResponseCacheService
	{
		private readonly IDatabase _RedisDb;

		public ResponseCacheService(IConnectionMultiplexer redis)
		{
			_RedisDb=redis.GetDatabase();
			
		}
		public async Task CacheResponseAsync(string key, object items, TimeSpan LiveTime)
		{
			if (items is null) return;

			var options=new JsonSerializerOptions() { PropertyNamingPolicy= JsonNamingPolicy.CamelCase };
			var json= JsonSerializer.Serialize(items,options);
			await _RedisDb.StringSetAsync(key, json, LiveTime);
			
		}

		public async Task<string> GetResponseCacheAsync(string key)
		{

			var Resp= await _RedisDb.StringGetAsync(key);
			if (Resp.IsNullOrEmpty) return null;
			return Resp;
		}
	}
}
