using Contracts;
using MassTransit;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class Publisher : BackgroundService
{
    private readonly IBus _bus;

    public Publisher(IBus bus)
    {
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Vòng lặp này sẽ tiếp tục chạy cho đến khi service bị hủy.
        while (!stoppingToken.IsCancellationRequested)
        {
            await _bus.Publish(new BurgerCookerOrderedEvent 
            {
                CorrelationId = Guid.NewGuid(),
                CookTemp = "Medium", 
                CustomerName="TanPham"
            }, stoppingToken);

            await Task.Delay(10000, stoppingToken);
        }
    }
}

