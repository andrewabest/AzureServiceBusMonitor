using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace ServiceBusMonitor.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            const string connectionString = "";
            const string queueName = "main-queue";

            var client = QueueClient.CreateFromConnectionString(connectionString, queueName, ReceiveMode.PeekLock);

            client.OnMessageAsync(async message =>
            {
                Console.WriteLine("Received Message:");
                Console.WriteLine($"Message body: {message.GetBody<String>()}");
                Console.WriteLine($"Message id: {message.MessageId}");
                Console.WriteLine($"DeliveryCount: {message.DeliveryCount}");

                await message.AbandonAsync();

                // await message.CompleteAsync();

            }, new OnMessageOptions {AutoComplete = false});

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
