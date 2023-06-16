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

		public static void AddServices(IServiceCollection services)
		{
			string connectionString = Configuration.GetConnectionString("DefaultConnection");
			_ = services.AddDbContext<TripsDBContext>(options =>
				options.UseSqlServer(connectionString));
			_ = services.AddScoped<IUnitOfWork, UnitOfWork>();
			_ = services.AddScoped<IUserService, UserService>();
			_ = services.AddScoped<ICommentService, CommentService>();
			_ = services.AddScoped<IImageService, ImageService>();
			_ = services.AddScoped<IRoutePointService, RoutePointService>();
			_ = services.AddScoped<ITripService, TripService>();
			_ = services.AddScoped<IDriverService, DriverService>();
			_ = services.AddScoped<IFeedbackService, FeedbackService>();
		}

		public static void AddAuthentication(IServiceCollection services)
		{
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
		}

		public static void AddJwtTokenToRequests(WebApplication app)
		{
			_ = app.Use(async (context, next) =>
			{
				string? jwtToken = context.Request.Cookies[Constants.JwtTokenCookiesAlias];
				if (!string.IsNullOrEmpty(jwtToken))
				{
					context.Request.Headers.Add("Authorization", "Bearer " + jwtToken);
				}
				await next();
			});
		}

		public static void AddUnauthorizedStateRedirection(WebApplication app)
		{
			_ = app.UseStatusCodePages(async context =>
			{
				HttpRequest request = context.HttpContext.Request;
				HttpResponse response = context.HttpContext.Response;

				if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
				{
					response.Redirect("/Account/Login");
				}
			});
		}
	}
}
