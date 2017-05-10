using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace ServiceBusMonitor
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

            var client = QueueClient.CreateFromConnectionString(connectionString,
                QueueClient.FormatDeadLetterPath(queueName), ReceiveMode.PeekLock);

            var currentDeadLetters = (await client.ReceiveBatchAsync(100)).ToList();

            if (currentDeadLetters.Any())
            {
                Console.WriteLine($"Current dead letters in dead letter queue:");
            }

            foreach (var message in currentDeadLetters)
            {
                Console.WriteLine($"Received Dead Letter body: {message.GetBody<string>()}");
                Console.WriteLine($"Received Dead Letter id: {message.MessageId}");
            }

            client.OnMessageAsync(async message =>
            {
                Console.WriteLine("Received Dead Letter:");
                Console.WriteLine($"Received Dead Letter body: {message.GetBody<string>()}");
                Console.WriteLine($"Received Dead Letter id: {message.MessageId}");

                await message.CompleteAsync();
            });

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
