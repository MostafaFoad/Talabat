using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talapat.Core.Entities.Order_Aggregation
{
	public class Order:BaseEntity
	{
		public Order()
		{
		}

		public Order(string byerEmail, Address shipingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal,string paymentIntentId)
		{
			BuyerEmail = byerEmail;
			ShippingAddress = shipingAddress;
			DeliveryMethod = deliveryMethod;
			Items = items;
			Subtotal = subTotal;
			PaymentIntentId = paymentIntentId;
		}

		public string BuyerEmail { get; set; }
		public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
		public OrderStatus Status { get; set; } = OrderStatus.Pending;
		public Address ShippingAddress { get; set; }
		public DeliveryMethod DeliveryMethod { get; set; }
		public ICollection<OrderItem> Items { get; set; }=new HashSet<OrderItem>();
		public decimal Subtotal { get; set; }
		public decimal Total { get=>Subtotal+DeliveryMethod.Cost; }
		public string PaymentIntentId { get; set; }

	}
}
