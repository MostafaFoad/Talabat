using AutoMapper;
using Microsoft.AspNetCore.Connections;
using Talabat.APIs.Dtos;
using Talapat.Core.Entities;
using Talapat.Core.Entities.Order_Aggregation;

namespace Talabat.APIs.Helper
{
	public class OrderItemResolver : IValueResolver<OrderItem, OrderItemDto, string>
	{
		private readonly IConfiguration _configuration;

		public OrderItemResolver(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
		{
			if (!string.IsNullOrEmpty(source.Product.PictureUrl))
			{
				return $"{_configuration["ApiBaseUrl"]}{source.Product.PictureUrl}";
			}
			return string.Empty;
		}
	}
}
