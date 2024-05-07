using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.MiddleWares
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;
		private readonly IHostEnvironment _env;

		public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger,IHostEnvironment env)
		{
			_next = next;
			_logger = logger;
			_env = env;
		}
		public async Task InvokeAsync(HttpContext Req)
		{
			try
			{
			 await	_next.Invoke(Req);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				Req.Response.StatusCode =(int) HttpStatusCode.InternalServerError;
				Req.Response.ContentType = "application/json";
				var servererror = _env.IsDevelopment()? new ServerError((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
								                      : new ServerError((int)HttpStatusCode.InternalServerError);
				var option = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
				var jsonresp=JsonSerializer.Serialize(servererror,option);
				await  Req.Response.WriteAsync(jsonresp);
			}
		}
	}
}
