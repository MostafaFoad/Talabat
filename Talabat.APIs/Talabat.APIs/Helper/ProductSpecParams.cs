namespace Talabat.APIs.Helper
{
	public class ProductSpecParams
	{
		private const int MaxPageSize = 10;
		private int pageize = 5;

		public int PageSize
		{
			get { return pageize; }
			set { pageize = value > MaxPageSize ? MaxPageSize : value; }
		}
		public int PageIndex { get; set; } = 1;
		private string search;

		public string? Search
		{
			get { return  search; }
			set {  search = value.ToLower(); }
		}

		
		public string? Sort { get; set; }
		public int? BrandId { get; set; }
		public int? TypeId { get; set; }
	}
}
