using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talapat.Core.Entities.Order_Aggregation;

namespace Talapat.Repository.DataBase.Config
{
	public class OrderItemConfi : IEntityTypeConfiguration<OrderItem>
	{
		public void Configure(EntityTypeBuilder<OrderItem> builder)
		{
			builder.Property(O => O.Price).HasColumnType("decimal(18,2)");
			builder.OwnsOne(o => o.Product, pitem => pitem.WithOwner());
		}
	}
}
