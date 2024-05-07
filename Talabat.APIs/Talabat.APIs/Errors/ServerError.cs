namespace Talabat.APIs.Errors
{
	public class ServerError:ApiResponse
	{
		public string? Details { get; set; }
		public ServerError(int code,string?mess=null,string? details=null ):base(code,mess)
		{
			Details = details;
		}
	}
}
