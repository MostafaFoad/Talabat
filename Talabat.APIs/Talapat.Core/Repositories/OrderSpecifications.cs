
using Talapat.Core.Entities.Order_Aggregation;
using Talapat.Core.Repositories;

namespace Talapat.Core.Repositories
{
	public class OrderSpecifications:BaseSpecification<Order>
	{
		public OrderSpecifications(string email):base(O=>O.BuyerEmail==email)
		{
			IncludeCrietria.Add(O=>O.DeliveryMethod);
			IncludeCrietria.Add(O=>O.Items);
			AddOrderByDesc(O=>O.OrderDate);
		}
		public OrderSpecifications(string email,int orderid) : base(O => O.BuyerEmail == email&&O.Id==orderid)
		{
			IncludeCrietria.Add(O => O.DeliveryMethod);
			IncludeCrietria.Add(O => O.Items);
		}
	}
}
