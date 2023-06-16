using Andrei_Mikhaleu_Task1;
using AutoMapper;
using System.Reflection;
using TripsServiceBLL.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ProgramHelper.AddServices(builder.Services);

_ = builder.Services.AddHttpContextAccessor();

_ = builder.Services.AddControllersWithViews();

ProgramHelper.AddAuthentication(builder.Services);
_ = builder.Services.AddAuthorization();

_ = builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

MapperConfiguration mapperConfig = new(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
_ = builder.Services.AddSingleton(mapper);

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    _ = app.UseExceptionHandler("/Home/Error");
    _ = app.UseHsts();
}

_ = app.UseHttpsRedirection();
_ = app.UseStaticFiles();

_ = app.UseRouting();

_ = app.UseCors("AllowAll");

ProgramHelper.AddJwtTokenToRequests(app);
ProgramHelper.AddUnauthorizedStateRedirection(app);

_ = app.UseAuthentication();
_ = app.UseAuthorization();

_ = app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
