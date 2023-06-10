using TripsServiceBLL.Services;
using TripsServiceDAL.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TripsServiceDAL.Interfaces;
using TripsServiceBLL.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;

namespace Andrei_Mikhaleu_Task1
{
	public static class ProgramHelper
	{
		public static IConfigurationRoot Configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: false)
				.Build();

		public static void ConfigureServices(IServiceCollection services)
		{
			string connectionString = Configuration.GetConnectionString("DefaultConnection");

			services.AddControllersWithViews();
			services.AddDbContext<TripsDBContext>(options =>
				options.UseSqlServer(connectionString));
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<ICommentService, CommentService>();
			services.AddScoped<IImageService, ImageService>();
			services.AddScoped<IRoutePointService, RoutePointService>();
			services.AddScoped<ITripService, TripService>();
			services.AddHttpContextAccessor();

			services.AddAuthentication(options =>
			{
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					RequireExpirationTime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = Configuration["Jwt:Issuer"],
					ValidAudience = Configuration["Jwt:Issuer"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
				};
			});

			services.AddAuthorization();
		}

		public static void Configure(WebApplication app)
		{
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseCors("AllowAll");

			app.Use(async (context, next) =>
			{
				string? jwtToken = context.Request.Cookies["jwt"];
				if (!string.IsNullOrEmpty(jwtToken))
				{
					context.Request.Headers.Add("Authorization", "Bearer " + jwtToken);
				}
				await next();
			});

			app.UseStatusCodePages(async context =>
			{
				var request = context.HttpContext.Request;
				var response = context.HttpContext.Response;

				if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
				{
					response.Redirect("/Account/Login");
				}
			});

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
