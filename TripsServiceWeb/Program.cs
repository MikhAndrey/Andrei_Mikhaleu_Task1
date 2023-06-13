using Andrei_Mikhaleu_Task1;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ProgramHelper.ConfigureServices(builder.Services);

WebApplication app = builder.Build();

ProgramHelper.Configure(app);
