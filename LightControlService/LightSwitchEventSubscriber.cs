using Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightControlService;

public class LightSwitchEventSubscriber : IConsumer<LightSwitchEvent>
{
    public async Task Consume(ConsumeContext<LightSwitchEvent> context)
    {
        var lightEvent = context.Message;
        var isSuccessful = await ControleLightAsync(lightEvent);
        string lightMessage = lightEvent.State ? "ON" : "OFF" ;


        if (isSuccessful)
        {
            Console.WriteLine($"Lights switched {lightMessage} successfuly");
        }
        else
        {
            Console.WriteLine($"Failed to control lights");
        }
    }

    public static async Task<bool> ControleLightAsync(LightSwitchEvent lightEvent)
    {
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(2));

            if(lightEvent.State == true)
            {
                Console.WriteLine("Turning lights ON...");
            }
            else if (lightEvent.State == false)
            {
                Console.WriteLine("Turning lights OFF...");
            }

            return true;

        }
        catch (Exception ex)
        {

            Console.WriteLine($"Error controlling lights: {ex.Message}");
            return false;
        }
    }


}

