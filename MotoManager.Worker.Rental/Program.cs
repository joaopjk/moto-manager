using MotoManager.IoC.DI;
using MotoManager.Worker.MotocycleCreate;
using MotoManager.Worker.MotocycleCreate.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWorkerDependencyInjection();
builder.Services.AddAutoMapper();

builder.Services.AddSingleton<IWorkerService, WorkerService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
