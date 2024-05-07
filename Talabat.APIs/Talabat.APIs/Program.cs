using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helper;
using Talabat.APIs.MiddleWares;
using Talapat.Core.Entities.Identity;
using Talapat.Core.Repositories;
using Talapat.Repository;
using Talapat.Repository.DataBase;
using Talapat.Repository.Identity;

namespace Talabat.APIs
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			#region ConfigureServices

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddSwaggerServices();
			builder.Services.AddDbContext<StoreContext>(option =>
			{
				option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});
			builder.Services.AddDbContext<AppIdentityDbContext>(option =>
			{
				option.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
			});

			builder.Services.AddSingleton<IConnectionMultiplexer>(op =>
			{
			var conect = builder.Configuration.GetConnectionString("Redis");
			return ConnectionMultiplexer.Connect(conect);
			});
			builder.Services.AddAppServices();
			builder.Services.AddIdentityServices(builder.Configuration);
			builder.Services.AddCors(op =>
			{
				op.AddPolicy("MyPolicy", option =>
				{
					option.AllowAnyHeader().AllowAnyMethod().WithOrigins(builder.Configuration["FronBaseUrl"]);
				});
			});
			#endregion

			var app = builder.Build();
			
			using var scope = app.Services.CreateScope();

			var services=scope.ServiceProvider;

			var loggerfactory=services.GetRequiredService<ILoggerFactory>();
				
			try
			{
				var dbcontext = services.GetRequiredService<StoreContext>();
				await dbcontext.Database.MigrateAsync();
				await StoreContexSeed.SeedAsync(dbcontext);

				var Identitydbcontext = services.GetRequiredService<AppIdentityDbContext>();
				await Identitydbcontext.Database.MigrateAsync();
				var usermanager = services.GetRequiredService<UserManager<AppUser>>();
				await IdentityDbContextSeed.SeedUserAsync(usermanager);

			}
			catch(Exception ex)
			{
				var logger = loggerfactory.CreateLogger<Program>();
				logger.LogError(ex, ex.Message);
			}

			#region Kestral Pipline
			// Configure the HTTP request pipeline.
			app.UseMiddleware<ExceptionMiddleware>();

			if (app.Environment.IsDevelopment())
			{
				app.AddSwaggerMiddleware();
			}

			app.UseStatusCodePagesWithReExecute("/Errors/{0}");

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseCors("MyPolicy");

			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers(); 
			#endregion

			app.Run();
		}
	}
}