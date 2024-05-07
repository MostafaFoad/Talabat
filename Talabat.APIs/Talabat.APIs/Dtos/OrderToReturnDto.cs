using StackExchange.Redis;
using Talapat.Core.Entities.Order_Aggregation;

namespace Talabat.APIs.Dtos
{
	public class OrderToReturnDto
	{
		public int Id { get; set; }
		public string BuyerEmail { get; set; }
		public DateTimeOffset OrderDate { get; set; }
		public string Status { get; set; }
		public Address ShippingAddress { get; set; }
		public string DeliveryMethod { get; set; }
		public decimal DeliveryCost { get; set; }
		public IReadOnlyList<OrderItemDto> Items { get; set; }
		public decimal Subtotal { get; set; }
		public decimal Total { get; set; }
		public string PaymentIntentId { get; set; }
	}
}
