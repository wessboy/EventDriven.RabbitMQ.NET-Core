

using LightControlService;
using MassTransit;
//create an instance of MassTransit bus control
//allows the application to connect to RabbitMQ and recive events
var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{         //naming the Queue "lights"
          
    cfg.ReceiveEndpoint("lights",e => 
    {
        //specifiy the consumers of the Queue
        e.Consumer<LightSwitchEventSubscriber>();
    });
});


await busControl.StartAsync();


Console.WriteLine("Light control service is running press any key to exit.");

Console.ReadLine();
await busControl.StopAsync();