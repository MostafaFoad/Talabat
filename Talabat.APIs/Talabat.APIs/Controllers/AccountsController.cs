using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talapat.Core.Entities.Identity;
using Talapat.Core.Services;

namespace Talabat.APIs.Controllers
{

	public class AccountsController : BaseApiController
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ITokenServices _tokenServices;
		private readonly IMapper _mapper;

		public AccountsController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,
			ITokenServices tokenServices,IMapper mapper)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_tokenServices = tokenServices;
			_mapper = mapper;
		}
		[HttpPost("register")]
		public async Task<ActionResult<UserDto>> Register(RegisterDto model)
		{
			if (CheckEmailExist(model.Email).Result.Value) return BadRequest(new ApiResponse(400,"this email exist"));
			var user = new AppUser()
			{
				Email = model.Email,
				DisplayName = model.DisplayName,
				PhoneNumber = model.PhoneNumber,
				UserName = model.Email.Split('@')[0]
			};
			var res = await _userManager.CreateAsync(user, model.Password);
			if (!res.Succeeded) return BadRequest(new ApiResponse(400));
			return Ok(new UserDto() { DisplayName = user.DisplayName,
				Email = user.Email,
				Token = await _tokenServices.CreateTokenAsync(user,_userManager) });

		}

		[HttpPost("Login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null) return Unauthorized(new ApiResponse(401));
			var res=await _signInManager.CheckPasswordSignInAsync(user, model.Password,false);
			if(!res.Succeeded) return Unauthorized(new ApiResponse(401));
		    return Ok(new UserDto() { DisplayName=user.DisplayName,Email=user.Email
				,Token = await _tokenServices.CreateTokenAsync(user, _userManager)
			});
		}

		[Authorize]
		[HttpGet]
		public async Task<ActionResult<UserDto>> GetUser()
		{
			var email = User.FindFirstValue(ClaimTypes.Email);

			var user = await _userManager.FindByEmailAsync(email);
			return Ok(new UserDto()
			{
				DisplayName = user.DisplayName,
				Email = user.Email
				,
				Token = await _tokenServices.CreateTokenAsync(user, _userManager)
			});
		}
		[Authorize]
		[HttpGet("address")]
		public async Task<ActionResult<AddressDto>> GetUserAddress()
		{

			var user = await _userManager.FindUserAddress(User);
			return Ok(_mapper.Map<Address, AddressDto>(user.Address));
		}

		[Authorize]
		[HttpPut("address")]
		public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto dto)
		{

			var user = await _userManager.FindUserAddress(User);
			var add = _mapper.Map<AddressDto, Address>(dto);
			add.Id=user.Address.Id;
			user.Address = add;
			var res = await _userManager.UpdateAsync(user);
			if (!res.Succeeded) return BadRequest(new ApiResponse(400));
			return Ok(dto);
		}
		[HttpPut("emailexist")]
		public async Task<ActionResult<bool>> CheckEmailExist(string email)
		{
			return await _userManager.FindByEmailAsync(email) is not null;
		}
	}
}
