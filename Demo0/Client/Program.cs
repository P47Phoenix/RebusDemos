using System;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Logging;
using Rebus.Routing.TypeBased;

namespace Client
{
    class Program
    {
        static void Main()
        {
            using (var activator = new BuiltinHandlerActivator())
            {
                var bus = Configure.With(activator)
                    .Logging(l => l.ColoredConsole())
                    .Transport(t => t.UseAmazonSQS("", "", Amazon.RegionEndpoint.USWest2, "server", new AmazonSQSTransportOptions
                    {
                        ReceiveWaitTimeSeconds = 20
                    }))
                    .Routing(t => t.TypeBased().Map<string>("server"))
                    .Start();

                Console.WriteLine();
                while (true)
                {

                    Console.Write("> ");
                    var text = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(text)) break;

                    bus.Send(text);
                }
            }
        }
    }
}
