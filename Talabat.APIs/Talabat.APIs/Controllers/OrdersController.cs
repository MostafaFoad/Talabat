using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talapat.Core.Entities.Order_Aggregation;
using Talapat.Core.Repositories;
using Talapat.Core.Services;

namespace Talabat.APIs.Controllers
{
	[Authorize]
	public class OrdersController : BaseApiController
	{
		private readonly IOrderServices _orderServices;
		private readonly IMapper _mapper;

		public OrdersController(IOrderServices orderServices,IMapper mapper)
		{
			_orderServices = orderServices;
			_mapper = mapper;
		}
		
		[HttpPost]
		public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
		{
			var email=User.FindFirstValue(ClaimTypes.Email);
			var address = _mapper.Map<AddressDto, Address>(orderDto.shipToAddress);
			var order=await _orderServices.CreateOrderAsync(email, orderDto.BasketId,orderDto.DeliveryMethodId,address);
			if (order is null) return BadRequest(new ApiResponse(400));
			var orderdto=_mapper.Map<Order, OrderToReturnDto>(order);
			return Ok(orderdto);
		}

		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var orders = await _orderServices.GetOrdersForUserAsync(email);
			var orderdto = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList< OrderToReturnDto > >(orders);
			return Ok(orderdto);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id)
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var order = await _orderServices.GetOrderByIdForUserAsync(email, id);
			if (order is null) return NotFound(new ApiResponse(404));
			var orderdto = _mapper.Map<Order, OrderToReturnDto>(order);
			return Ok(orderdto);

		}

		[HttpGet("deliverymethods")]
		public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliverMethod()
		{
			var deliverymethods=await _orderServices.GetDeliveryMethodAsync();
			return Ok(deliverymethods);
		}
	}
}
