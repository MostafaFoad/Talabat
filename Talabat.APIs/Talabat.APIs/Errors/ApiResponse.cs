namespace Talabat.APIs.Errors
{
	public class ApiResponse
	{
		public int StatusCode { get; set; }
		public string? Messege { get; set; }
		public ApiResponse(int code,string? messeg=null)
		{
			StatusCode= code;
			Messege = messeg is null ? GetDefaultMessege(code) : messeg;
		}

		private string? GetDefaultMessege(int code)
		{
			return StatusCode switch
			{
				400 => "A Bad Request, You have made",
				401 => "Authorized you arn't",
				404 => "Not Found Resources",
				500=>"Errors are the path to success",
				_ => null

			};
		}
	}
}
