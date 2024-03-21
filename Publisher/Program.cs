using Events;
using MassTransit;
using MassTransit.Initializers.TypeConverters;



var busControl = Bus.Factory.CreateUsingRabbitMq();

await busControl.StartAsync();

Console.WriteLine("Home Automation system is running.");

while (true)
{
    Console.WriteLine("Choose an action:");
    Console.WriteLine("1. Turn Lights On");
    Console.WriteLine("2. Turn Lights Off");
    Console.WriteLine("3. Adjust Thermostat");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            await ControllLights(busControl,true); break;
        case "2": 
            await ControllLights(busControl,false); break;
        case "3":
            Console.WriteLine("Enter new thermostat temperature:");
            if (decimal.TryParse(Console.ReadLine(),out decimal temperature))

            {
                await ControlTermostat(busControl,temperature);  
            }
            else
            {
                Console.WriteLine("Invalid temperature input.");
            }
            break;
        default:
            Console.WriteLine("Please select out of the given options.");
            break;
    }
}
































//controlling the light switch on or off (publish a message in the "lights" Queue
static async Task ControllLights(IBusControl busControl,bool state)
{
    var lightEndpoint = await busControl.GetSendEndpoint(new Uri("rabbitmq://localhost/lights"));
    await lightEndpoint.Send<LightSwitchEvent>(new LightSwitchEvent(new Guid(),state));
    string stateMessage = state ? "ON" : "OFF"; 
    Console.WriteLine($"Lights switched {stateMessage}");
}

//controlling Temperature (publish a message in Thermostat Queue

static async Task ControlTermostat(IBusControl busControl,decimal temperature)
{
    var thermostatEndpoint = await busControl.GetSendEndpoint(new Uri("rabbitmq://localhost/thermostat"));
    await thermostatEndpoint.Send<ThermostatTempChangeEvent>(new ThermostatTempChangeEvent(new Guid(),temperature));

    Console.WriteLine($"Thermostat adjusted to {temperature} °C");
}