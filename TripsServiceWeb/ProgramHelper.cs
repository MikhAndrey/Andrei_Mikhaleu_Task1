using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TripsServiceBLL.Commands.Chats;
using TripsServiceBLL.Commands.Trips;
using TripsServiceBLL.Commands.Users;
using TripsServiceBLL.Infrastructure.Mappers;
using TripsServiceBLL.Infrastructure.ValueResolvers;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Services;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Interfaces;

namespace Andrei_Mikhaleu_Task1;

public static class ProgramHelper
{
	public static IConfigurationRoot Configuration = new ConfigurationBuilder()
		.AddJsonFile("appsettings.json", false)
		.Build();

	public static void AddServices(IServiceCollection services)
	{
		string connectionString = Configuration.GetConnectionString("DefaultConnection");
		services.AddDbContext<TripsDBContext>(options =>
			options.UseSqlServer(connectionString));
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<IUserService, UserService>();
		services.AddScoped<ICommentService, CommentService>();
		services.AddScoped<IImageService, ImageService>();
		services.AddScoped<IRoutePointService, RoutePointService>();
		services.AddScoped<ITripService, TripService>();
		services.AddScoped<IDriverService, DriverService>();
		services.AddScoped<IFeedbackService, FeedbackService>();
		services.AddScoped<IRoleService, RoleService>();
		services.AddScoped<IChatService, ChatService>();
		services.AddSingleton<INotificationsService, NotificationsService>();
	}

	public static void AddValueResolvers(IServiceCollection services)
	{
		services.AddScoped<CurrentUserTripResolver>();
		services.AddScoped<CommentUserIdResolver>();
		services.AddScoped<FeedbackUserResolver>();
		services.AddScoped<NewTripUserIdResolver>();
		services.AddScoped<TripImageLinkResolver>();
		services.AddScoped<DriverImageLinkResolver>();
		services.AddScoped<NewUserRoleIdResolver>();
		services.AddScoped<CurrentUserChatResolver>();
	}

	public static void AddCommands(IServiceCollection services)
	{
		services.AddScoped<CreateTripCommandAsync>();
		services.AddScoped<DeleteTripCommandAsync>();
		services.AddScoped<EditTripCommandAsync>();
		services.AddScoped<EditPastTripCommandAsync>();
		services.AddScoped<LoginUserCommand>();
		services.AddScoped<ChatCreateCommand>();
		services.AddScoped<ChatJoiningCommand>();
		services.AddScoped<ChatDeleteCommand>();
	}

	public static void AddMapper(IServiceCollection services)
	{
		ServiceProvider serviceProvider = services.BuildServiceProvider();

		MapperConfiguration mapperConfig = new(mc =>
		{
			mc.ConstructServicesUsing(serviceProvider.GetService);
			mc.AddProfile(new TripMapper(
				serviceProvider.GetService<CurrentUserTripResolver>(),
				serviceProvider.GetService<NewTripUserIdResolver>(),
				serviceProvider.GetService<TripImageLinkResolver>()));
			mc.AddProfile(new CommentMapper(serviceProvider.GetService<CommentUserIdResolver>()));
			mc.AddProfile(new DriverMapper(serviceProvider.GetService<DriverImageLinkResolver>()));
			mc.AddProfile(new FeedbackMapper(serviceProvider.GetService<FeedbackUserResolver>()));
			mc.AddProfile(new ImageMapper());
			mc.AddProfile(new RoutePointMapper());
			mc.AddProfile(new UserMapper(serviceProvider.GetService<NewUserRoleIdResolver>()));
			mc.AddProfile(new ChatMapper(serviceProvider.GetService<CurrentUserChatResolver>()));
		});

		IMapper mapper = mapperConfig.CreateMapper();
		services.AddSingleton(mapper);
	}

	public static void AddAuthentication(IServiceCollection services)
	{
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
				ValidIssuer = UtilConstants.JwtIssuer,
				ValidAudience = UtilConstants.JwtIssuer,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(UtilConstants.JwtKey))
			};
		});
	}

	public static void AddJwtTokenToRequests(WebApplication app)
	{
		app.Use(async (context, next) =>
		{
			string? jwtToken = context.Request.Cookies[UtilConstants.JwtTokenCookiesAlias];
			if (!string.IsNullOrEmpty(jwtToken))
			{
				context.Request.Headers.Add("Authorization", "Bearer " + jwtToken);
			}

			await next();
		});
	}
}
