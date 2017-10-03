using System;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Logging;

#pragma warning disable 1998

namespace Server
{
    class Program
    {
        static void Main()
        {
            using (var activator = new BuiltinHandlerActivator())
            {
                activator.Handle<string>(async message =>
                {
                    Console.WriteLine($"Received message {message}");
                });

                Configure.With(activator)
                    .Logging(l => l.ColoredConsole())
                    .Transport(t => t.UseAmazonSQS("", "", Amazon.RegionEndpoint.USWest2, "server", new AmazonSQSTransportOptions
                    {
                        ReceiveWaitTimeSeconds = 20
                    })).Start();

                Console.WriteLine("Press ENTER to quit");

                Console.ReadLine();
            }
        }
    }
}
