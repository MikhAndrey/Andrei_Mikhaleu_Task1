using Andrei_Mikhaleu_Task1;
using AutoMapper;
using System.Reflection;
using TripsServiceBLL.Infrastructure.Mappers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

ProgramHelper.AddServices(builder.Services);
ProgramHelper.AddCommands(builder.Services);

builder.Services.AddControllersWithViews();

ProgramHelper.AddAuthentication(builder.Services);
builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

MapperConfiguration mapperConfig = new(mc =>
{
    mc.AddProfile(new TripMapper());
    mc.AddProfile(new CommentMapper());
    mc.AddProfile(new DriverMapper());
    mc.AddProfile(new FeedbackMapper());
    mc.AddProfile(new ImageMapper());
    mc.AddProfile(new RoutePointMapper());
    mc.AddProfile(new UserMapper());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAll");

ProgramHelper.AddJwtTokenToRequests(app);
ProgramHelper.AddUnauthorizedStateRedirection(app);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
