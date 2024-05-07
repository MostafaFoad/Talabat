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
	public class OrderConfigurations : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.OwnsOne(O => O.ShippingAddress, Add => Add.WithOwner());
			builder.Property(O => O.Status).HasConversion(
				OstatInDb => OstatInDb.ToString(),
				RecivedStat => (OrderStatus) Enum.Parse(typeof(OrderStatus), RecivedStat)
				);
			builder.Property(O => O.Subtotal).HasColumnType("decimal(18,2)");
			builder.HasMany(o=>o.Items).WithOne().OnDelete(DeleteBehavior.Cascade);
		}
	}
}
