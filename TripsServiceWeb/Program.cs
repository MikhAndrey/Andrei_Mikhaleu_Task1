using Andrei_Mikhaleu_Task1;

var builder = WebApplication.CreateBuilder(args);

ProgramHelper.ConfigureServices(builder.Services);

var app = builder.Build();

ProgramHelper.Configure(app);
