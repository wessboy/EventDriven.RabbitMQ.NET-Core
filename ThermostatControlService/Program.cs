
using MassTransit;
using ThermostatControlService;

var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.ReceiveEndpoint("thermostat", e =>
    {
        e.Consumer<ThermostatTempChangeEventSubscriber>();
    });
});

await busControl.StartAsync();

Console.WriteLine("Thermostat control service is running. Press any key to exit. ");

Console.ReadLine();

await busControl.StopAsync();   