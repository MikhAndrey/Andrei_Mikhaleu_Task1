using System.Reflection;
using Andrei_Mikhaleu_Task1;
using Andrei_Mikhaleu_Task1.Hubs;
using Microsoft.AspNetCore.SignalR;
using TripsServiceBLL.Infrastructure.Common;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

ProgramHelper.AddServices(builder.Services);
ProgramHelper.AddValueResolvers(builder.Services);
ProgramHelper.AddCommands(builder.Services);
ProgramHelper.AddSchedulers(builder.Services);

builder.Services.AddControllersWithViews();

ProgramHelper.AddAuthentication(builder.Services);
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendHostCorsPolicy", policyBuilder =>
    {
        policyBuilder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins("https://localhost:44402");
    });
});

builder.Services.AddSignalR();
builder.Services.AddMemoryCache();

builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

ProgramHelper.AddMapper(builder.Services);

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("FrontendHostCorsPolicy");

ProgramHelper.AddJwtTokenToRequests(app);

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<FeedbacksHub>("/feedbackshub");
app.MapHub<ChatHub>("/chathub");
app.MapHub<NotificationHub>("/notificationhub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.Run();
