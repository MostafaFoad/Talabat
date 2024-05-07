using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talapat.Repository.DataBase;

namespace Talabat.APIs.Controllers
{
	
	public class buggyController : BaseApiController
	{
		private readonly StoreContext _store;

		public buggyController(StoreContext store)
		{
			_store = store;
		}
		[HttpGet("notfound")]
		public ActionResult notfound()
		{
			var p=_store.Products.Find(100);
			if (p is null) return NotFound(new ApiResponse(404));
			return Ok(p);
		}

		[HttpGet("servererror")]
		public ActionResult ServerError()
		{
			var p = _store.Products.Find(100);
			int i = p.Id;
			return Ok(p);
		}

		[HttpGet("badrequest")]
		public ActionResult BadError()
		{
			return BadRequest(new ApiResponse(400)) ;
		}

		[HttpGet("badrequest/{id}")]
		public ActionResult BadError(int id)
		{
			return BadRequest();
		}

	}
}
