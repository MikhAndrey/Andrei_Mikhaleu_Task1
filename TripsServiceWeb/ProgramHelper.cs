﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;
using TripsServiceBLL.Commands.Trips;
using TripsServiceBLL.Infrastructure.Mappers;
using TripsServiceBLL.Infrastructure.ValueResolvers;
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
        }

        public static void AddValueResolvers(IServiceCollection services)
        {
            services.AddScoped<CurrentUserTripResolver>();
            services.AddScoped<CommentUserIdResolver>();
        }

        public static void AddCommands(IServiceCollection services)
        {
            services.AddScoped<CreateTripCommandAsync>();
            services.AddScoped<DeleteTripCommandAsync>();
            services.AddScoped<EditTripCommandAsync>();
            services.AddScoped<EditPastTripCommandAsync>();
        }

        public static void AddMapper(IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            MapperConfiguration mapperConfig = new(mc =>
            {
                mc.ConstructServicesUsing(serviceProvider.GetService);
                mc.AddProfile(new TripMapper(serviceProvider.GetService<CurrentUserTripResolver>()));
                mc.AddProfile(new CommentMapper(serviceProvider.GetService<CommentUserIdResolver>()));
                mc.AddProfile(new DriverMapper());
                mc.AddProfile(new FeedbackMapper());
                mc.AddProfile(new ImageMapper());
                mc.AddProfile(new RoutePointMapper());
                mc.AddProfile(new UserMapper());
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

        public static void AddUnauthorizedStateRedirection(WebApplication app)
        {
            app.UseStatusCodePages(async context =>
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
