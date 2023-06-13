using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Services;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Interfaces;

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

			_ = services.AddControllersWithViews();
			_ = services.AddDbContext<TripsDBContext>(options =>
				options.UseSqlServer(connectionString));
			_ = services.AddScoped<IUnitOfWork, UnitOfWork>();
			_ = services.AddScoped<IUserService, UserService>();
			_ = services.AddScoped<ICommentService, CommentService>();
			_ = services.AddScoped<IImageService, ImageService>();
			_ = services.AddScoped<IRoutePointService, RoutePointService>();
			_ = services.AddScoped<ITripService, TripService>();
			_ = services.AddHttpContextAccessor();

			_ = services.AddAuthentication(options =>
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
					ValidIssuer = Constants.JwtIssuer,
					ValidAudience = Constants.JwtIssuer,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.JwtKey))
				};
			});

			_ = services.AddAuthorization();
		}

		public static void Configure(WebApplication app)
		{
			if (!app.Environment.IsDevelopment())
			{
				_ = app.UseExceptionHandler("/Home/Error");
				_ = app.UseHsts();
			}

			_ = app.UseHttpsRedirection();
			_ = app.UseStaticFiles();

			_ = app.UseRouting();

			_ = app.UseCors("AllowAll");

			_ = app.Use(async (context, next) =>
			{
				string? jwtToken = context.Request.Cookies[Constants.JwtTokenCookiesAlias];
				if (!string.IsNullOrEmpty(jwtToken))
				{
					context.Request.Headers.Add("Authorization", "Bearer " + jwtToken);
				}
				await next();
			});

			_ = app.UseStatusCodePages(context =>
			{
				HttpRequest request = context.HttpContext.Request;
				HttpResponse response = context.HttpContext.Response;

				if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
				{
					response.Redirect("/Account/Login");
				}
			});

			_ = app.UseAuthentication();
			_ = app.UseAuthorization();

			_ = app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
