using WorkerService.Services;
using Microsoft.EntityFrameworkCore;
using WorkerService;
using WorkerService.Data;
using WorkerService.Repositories;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("Database")));

builder.Services.AddScoped<IPacketRepository, PacketRepository>();
////builder.Services.AddScoped<ICaptureService, CaptureService>();
builder.Services.AddScoped<IPacketManager, PacketManager>();


builder.Services.AddHostedService<Worker>();

var host = builder.Build();

host.Run();
