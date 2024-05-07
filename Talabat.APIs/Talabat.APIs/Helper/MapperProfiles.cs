using AutoMapper;
using Talabat.APIs.Dtos;
using Talapat.Core.Entities;
using Talapat.Core.Entities.Identity;
using Talapat.Core.Entities.Order_Aggregation;

namespace Talabat.APIs.Helper
{
	public class MapperProfiles:Profile
	{
		public MapperProfiles()
		{
			CreateMap<Product, ProductToReturnDto>()
				.ForMember(D => D.ProductBrand, O => O.MapFrom(S => S.ProductBrand.Name))
				.ForMember(D => D.ProductType, O => O.MapFrom(S => S.ProductType.Name))
				.ForMember(D => D.PictureUrl, O => O.MapFrom< ProductPicturUrlResolver>());
			CreateMap<Talapat.Core.Entities.Identity.Address, AddressDto>().ReverseMap();
			CreateMap<CustomerBasketDto, CustomerBasket>();
			CreateMap<BasketItemDto, BasketItem>();
			CreateMap<AddressDto, Talapat.Core.Entities.Order_Aggregation.Address>();

			CreateMap<OrderItem, OrderItemDto>()
				.ForMember(d=>d.ProductId,o=>o.MapFrom(s=>s.Product.ProductId))
				.ForMember(d=>d.ProductName,o=>o.MapFrom(s=>s.Product.ProductName))
				.ForMember(d=>d.PictureUrl,o=>o.MapFrom<OrderItemResolver>());

			CreateMap<Order, OrderToReturnDto>()
				.ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
				.ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
				.ForMember(d => d.DeliveryCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));
				


		}
	}
}
