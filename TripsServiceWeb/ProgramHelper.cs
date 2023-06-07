using Microsoft.AspNetCore.Authentication.Cookies;
using TripsServiceBLL.Services;
using TripsServiceDAL.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Andrei_Mikhaleu_Task1
{
    public static class ProgramHelper
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddControllersWithViews();
            services.AddDbContext<TripsDBContext>(options =>
                    options.UseSqlServer(connectionString));
            services.AddScoped<UnitOfWork>();
            services.AddScoped<UserService>();
            services.AddScoped<CommentService>();
            services.AddScoped<ImageService>();
            services.AddScoped<RoutePointService>();
            services.AddScoped<TripService>();
            services.AddHttpContextAccessor();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => {
                    options.LoginPath = "/Login/Index";
                    options.LogoutPath = "/Login/Logout";
                });
        }
    }
}
