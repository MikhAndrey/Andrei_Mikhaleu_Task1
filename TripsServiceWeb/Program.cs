using Andrei_Mikhaleu_Task1;
using AutoMapper;
using System.Reflection;
using TripsServiceBLL.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ProgramHelper.AddServices(builder.Services);

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();

ProgramHelper.AddAuthentication(builder.Services);
builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

MapperConfiguration mapperConfig = new(mc =>
{
    mc.AddProfile(new MappingProfile());
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
