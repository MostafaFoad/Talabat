using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helper;
using Talabat.APIs.Specifications;
using Talapat.Core.Entities;
using Talapat.Core.Repositories;
using Talapat.Core.Services;

namespace Talabat.APIs.Controllers
{
	
	public class ProductsController : BaseApiController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ProductsController(IUnitOfWork unitOfWork,IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		[CachedAttribute(600)]
		[HttpGet]
		public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams specparm)
		{
			var Spec = new ProductSpecifications(specparm);
			var product=await _unitOfWork.CreateRepo<Product>().GetAllWithSpecAsync( Spec);
			var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(product);
			var countspec = new ProductWithCountSpec(specparm);
			int count = await _unitOfWork.CreateRepo<Product>().GetConutWithSpecAsync(countspec);
			return Ok(new Pagination<ProductToReturnDto>(specparm.PageIndex,specparm.PageSize,count,data)); 
		}

		[ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
		{
			var Spec = new ProductSpecifications(id);
			var product = await _unitOfWork.CreateRepo<Product>().GetByIdWithSpecAsync(Spec);
			if (product is null) return NotFound(new ApiResponse(404));
			return Ok(_mapper.Map<Product,ProductToReturnDto>(product));
		}

		[HttpGet("brands")]
		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
		{
			
			var Brands = await _unitOfWork.CreateRepo<ProductBrand>().GetAllAsync();
			return Ok(Brands);
		}
		[HttpGet("types")]
		public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
		{

			var Types = await _unitOfWork.CreateRepo<ProductType>().GetAllAsync();
			return Ok(Types);
		}
	}
}
