using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talapat.Core.Entities;
using Talapat.Core.Entities.Order_Aggregation;

namespace Talapat.Repository.DataBase
{
	public static class StoreContexSeed
	{
		public static async Task SeedAsync(StoreContext context)
		{
			if (!context.ProductBrands.Any())
			{
				var brandData = File.ReadAllText("../Talapat.Repository/DataBase/DataSeeding/brands.json");
				var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
				if (brands is not null && brands.Count > 0)
				{
					foreach (var brand in brands)
					{
						await context.Set<ProductBrand>().AddAsync(brand);
					}
					await context.SaveChangesAsync();
				}
			}

			if (!context.ProductTypes.Any())
			{
				var TypeData = File.ReadAllText("../Talapat.Repository/DataBase/DataSeeding/types.json");
				var types = JsonSerializer.Deserialize<List<ProductType>>(TypeData);
				if (types is not null && types.Count > 0)
				{
					foreach (var type in types)
					{
						await context.Set<ProductType>().AddAsync(type);
					}
					await context.SaveChangesAsync();
				}
			}

			if (!context.Products.Any())
			{
				var productData = File.ReadAllText("../Talapat.Repository/DataBase/DataSeeding/products.json");
				var products = JsonSerializer.Deserialize<List<Product>>(productData);
				if (products is not null && products.Count > 0)
				{
					foreach (var product in products)
					{
						await context.Set<Product>().AddAsync(product);
					}
					await context.SaveChangesAsync();
				}
			}
			if (!context.DeliveryMethods.Any())
			{
				var deliveryjson = File.ReadAllText("../Talapat.Repository/DataBase/DataSeeding/delivery.json");
				var deliveries = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryjson);
				if (deliveries is not null && deliveries.Count > 0)
				{
					foreach (var delmeth in deliveries)
					{
						await context.Set<DeliveryMethod>().AddAsync(delmeth);
					}
					await context.SaveChangesAsync();
				}
			}
		}
	}
}
