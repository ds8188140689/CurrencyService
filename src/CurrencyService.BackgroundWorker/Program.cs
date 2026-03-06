using CurrencyService.BackgroundWorker.Services;
using CurrencyService.Shared.Application.Interfaces;
using CurrencyService.Shared.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient<CbrXmlParser>();
builder.Services.AddDbContext<IAppDbContext, AppDbContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddHostedService<CurrencyUpdateService>();

IHost host = builder.Build();

// Настройка отмены через Ctrl+C
using CancellationTokenSource cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

await host.RunAsync(cts.Token);