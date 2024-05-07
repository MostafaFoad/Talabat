using Talabat.APIs.Helper;
using Talapat.Core.Entities;
using Talapat.Core.Repositories;

namespace Talabat.APIs.Specifications
{
	public class ProductWithCountSpec:BaseSpecification<Product>
	{
		public ProductWithCountSpec(ProductSpecParams specparm) :
			base((P) => (!specparm.BrandId.HasValue || P.ProductBrandId == specparm.BrandId) &&
			(!specparm.TypeId.HasValue || P.ProductTypeId == specparm.TypeId)
			&& (string.IsNullOrEmpty(specparm.Search) || P.Name.ToLower().Contains(specparm.Search)))
		{

		}
	}
}
