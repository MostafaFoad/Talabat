
using Microsoft.AspNetCore.Identity;
using Talapat.Core.Entities.Identity;

using Talapat.Repository.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talapat.Core.Services;
using Talapat.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Talabat.APIs.Extensions
{
	public static class IdentityServices
	{
		public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration _configuration) {
			services.AddScoped<ITokenServices, TokenServices>();
			services.AddIdentity<AppUser, IdentityRole>(op =>
			{
				op.Password.RequireLowercase = true;
				op.Password.RequireUppercase = true;
				op.Password.RequireDigit = true;
				op.Password.RequireNonAlphanumeric = true;

			})
			.AddEntityFrameworkStores<AppIdentityDbContext>();
			services.AddAuthentication(op => {
				op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				op.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(op =>
				{
					op.TokenValidationParameters=new TokenValidationParameters() { 
					ValidateIssuer= true,
					ValidIssuer= _configuration["JWT:ValidIssuer"],
					ValidateAudience= true,
					ValidAudience= _configuration["JWT:ValidAudience"],
					ValidateLifetime= true,
					ValidateIssuerSigningKey= true,
					IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]))
					};
				});
			return services;

			}
	}
}
