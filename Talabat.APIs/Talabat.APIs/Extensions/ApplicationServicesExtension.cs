using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helper;
using Talapat.Core.Repositories;
using Talapat.Core.Services;
using Talapat.Repository;
using Talapat.Services;

namespace Talabat.APIs.Extensions
{
	public static class ApplicationServicesExtension
	{
		public static IServiceCollection AddAppServices(this IServiceCollection services)
		{
			//services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRopsitory<>));
			services.AddSingleton<IResponseCacheService, ResponseCacheService>();
			services.AddScoped<IPaymetService,PaymentService>();
			services.AddScoped<IUnitOfWork,UnitOfWork>();
			services.AddScoped<IOrderServices, OrderServices>();
			services.AddAutoMapper(typeof(MapperProfiles));
			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = (actionContext) =>
				{
					var errors = actionContext.ModelState.Where(M => M.Value.Errors.Count() > 0).SelectMany(E => E.Value.Errors)
					.Select(E => E.ErrorMessage).ToList();
					var validationReponse = new ApiValidationErrors() { Errors = errors };
					return new BadRequestObjectResult(validationReponse);
				};
			});
			services.AddScoped<IBasketRepository, BasketRepository>();
			return services;
		}
	}
}
