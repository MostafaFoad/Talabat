namespace Talabat.APIs.Errors
{
	public class ApiValidationErrors:ApiResponse
	{
		public IEnumerable<string> Errors { get; set; }
		public ApiValidationErrors():base(400)
		{
			Errors=new List<string>(); 
		}
	}
}
