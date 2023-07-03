using Andrei_Mikhaleu_Task1;
using Andrei_Mikhaleu_Task1.Hubs;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

ProgramHelper.AddServices(builder.Services);
ProgramHelper.AddValueResolvers(builder.Services);
ProgramHelper.AddCommands(builder.Services);

builder.Services.AddControllersWithViews();

ProgramHelper.AddAuthentication(builder.Services);
builder.Services.AddAuthorization();

builder.Services.AddSignalR();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

ProgramHelper.AddMapper(builder.Services);

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

app.MapHub<FeedbacksHub>("/feedbackshub");

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();
