using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talapat.Core.Services
{
	public interface IResponseCacheService
	{
		public Task<string> GetResponseCacheAsync(string key);
		 Task CacheResponseAsync(string key, object items,TimeSpan LiveTime);
	}
}
