using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talapat.Core.Entities.Identity;

namespace Talapat.Repository.Identity
{
	public static class IdentityDbContextSeed
	{
		public static async Task SeedUserAsync(UserManager<AppUser> userManager)
		{
			if (!userManager.Users.Any())
			{
				var user = new AppUser()
				{
					DisplayName = "Khaled Gamal",
					Email = "khaledelnassag@gmail.com",
					UserName = "khaledelnassa",
					PhoneNumber = "01159776769"
				};
				await userManager.CreateAsync(user,"Pa$$w0rd");
			}
		}
	}
}
