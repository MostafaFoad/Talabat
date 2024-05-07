using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talapat.Core.Services;

namespace Talabat.APIs.Helper
{
	public class CachedAttribute : Attribute, IAsyncActionFilter
	{
		private readonly int _liveTime;

		public CachedAttribute(int LiveTime)
		{
			_liveTime = LiveTime;
		}
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var _cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
			var key = GenerateCachKeyFromRequest(context.HttpContext.Request);
			 var cach=await _cacheService.GetResponseCacheAsync(key);

			if(!string.IsNullOrEmpty(cach)) {
				var content = new ContentResult()
				{
					Content = cach,
					ContentType = "application/json",
					StatusCode = 200
				};
				context.Result= content;
				return;
			}

			var ok=await next.Invoke();

			if(ok.Result is OkObjectResult objectResult)
			{
				await _cacheService.CacheResponseAsync(key, objectResult.Value,TimeSpan.FromSeconds(_liveTime));
			}

		}

		private string GenerateCachKeyFromRequest(HttpRequest request)
		{
			var build = new StringBuilder();
			build.Append(request.Path);
			foreach (var item in request.Query.OrderBy(q=>q.Key))
			{
				build.Append($"|{item.Key}-{item.Value}");
			}

			return build.ToString();
		}

	}
}
