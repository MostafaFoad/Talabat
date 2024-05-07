using AutoMapper;
using Microsoft.AspNetCore.Connections;
using Talabat.APIs.Dtos;
using Talapat.Core.Entities;

namespace Talabat.APIs.Helper
{
	public class ProductPicturUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
	{
		private readonly IConfiguration _configuration;

		public ProductPicturUrlResolver(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
		{
			if (!string.IsNullOrEmpty(source.PictureUrl))
			{
				return $"{_configuration["ApiBaseUrl"]}{source.PictureUrl}";
			}
			return string.Empty;
		}
	}
}
