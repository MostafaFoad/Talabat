using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talapat.Core.Entities.Order_Aggregation;

namespace Talapat.Core.Repositories
{
	public class OrderWithPaymentIdSpecification:BaseSpecification<Order>
	{
		public OrderWithPaymentIdSpecification(string paymentintent):base(o=>o.PaymentIntentId==paymentintent)
		{

		}
	}
}
