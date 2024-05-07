using Talabat.APIs.Helper;
using Talapat.Core.Entities;
using Talapat.Core.Repositories;

namespace Talabat.APIs.Specifications
{
	public class ProductSpecifications:BaseSpecification<Product>
	{
		public ProductSpecifications(ProductSpecParams specparm) :
			base((P) => (!specparm.BrandId.HasValue || P.ProductBrandId == specparm.BrandId) &&
			(!specparm.TypeId.HasValue || P.ProductTypeId == specparm.TypeId)
			&& (string.IsNullOrEmpty(specparm.Search) || P.Name.ToLower().Contains(specparm.Search)))

		{
			IncludeCrietria.Add(P => P.ProductBrand);
			IncludeCrietria.Add(P => P.ProductType);
			if (!string.IsNullOrEmpty(specparm.Sort))
			{
				switch (specparm.Sort)
				{
					case "priceAsc":
						AddOrderBy(P => P.Price);
						break;
					case "priceDesc":
						AddOrderByDesc(P => P.Price);
						break;
					default:
						AddOrderBy(P => P.Name);
						break;
				}                
			}

			ApplyPagination(specparm.PageSize,(specparm.PageIndex-1)*specparm.PageSize);
		}
		public ProductSpecifications(int id):base(P=>P.Id==id)
		{

		}
	}
}
