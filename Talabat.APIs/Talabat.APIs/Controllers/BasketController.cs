using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talapat.Core.Entities;
using Talapat.Core.Repositories;

namespace Talabat.APIs.Controllers
{
	public class BasketController : BaseApiController
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IMapper _mapper;

		public BasketController(IBasketRepository basketRepository,IMapper mapper)
		{
			_basketRepository = basketRepository;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string id)
		{
			var basket= await _basketRepository.GetBasketAsync(id);
			return basket is null?Ok(new CustomerBasket(id)):Ok(basket);

		}
		[HttpPost]
		public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(CustomerBasketDto basket)
		{
			var cust = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
			var baskt = await _basketRepository.UpdateBasketAsync(cust);
			return baskt is null ?BadRequest(new ApiResponse(400)):Ok(baskt);

		}

		[HttpDelete]
		public async Task<ActionResult<bool>> DeleteCustomerBasket(string id)
		{
			return await _basketRepository.DeleteBasketAsync(id);
			

		}
	}
}
