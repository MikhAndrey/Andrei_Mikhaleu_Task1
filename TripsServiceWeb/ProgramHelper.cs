using Microsoft.AspNetCore.Authentication.Cookies;
using TripsServiceBLL.Services;
using TripsServiceDAL.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TripsServiceDAL.Interfaces;
using TripsServiceBLL.Interfaces;
using Microsoft.CodeAnalysis;

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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login/Index";
                    options.LogoutPath = "/Login/Logout";
                });
        }

        public static void Configure (WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("AllowAll");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
