using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talapat.Core.Entities;

namespace Talapat.Repository.DataBase.Config
{
	internal class ProductConfiguration : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
			builder.Property(p => p.Name).HasMaxLength(100);

			builder.HasOne(p => p.ProductType).WithMany().HasForeignKey(p=>p.ProductTypeId);
			builder.HasOne(p => p.ProductBrand).WithMany().HasForeignKey(p => p.ProductBrandId);
		}
	}
}
