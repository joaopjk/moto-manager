using MotoManager.Domain.Interfaces.Events;
using MotoManager.Infrastructure.Events;
using MotoManager.Shared.Logger;
using MotoManager.Worker.ShowYear;
using MotoManager.Worker.ShowYear.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IRabbitMqEventHandler, RabbitMqEventHandler>();
builder.Services.AddSingleton(typeof(BaseLogger<>), typeof(BaseLogger<>));
builder.Services.AddSingleton<IWorkerService, WorkerShowYearService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
