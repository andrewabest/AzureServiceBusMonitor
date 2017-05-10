using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace ServiceBusMonitor.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "";
            const string queueName = "main-queue";

            var client = QueueClient.CreateFromConnectionString(connectionString, queueName);
            var source = new CancellationTokenSource();
            var token = source.Token;

            Task.Factory.StartNew(async () =>
            {
                token.ThrowIfCancellationRequested();

                while (true)
                {
                    Console.WriteLine("Sending test message.");
                    var message = new BrokeredMessage("This is a test message!");
                    await client.SendAsync(message);
                    Console.WriteLine("Test message sent, sleeping.");
                    Thread.Sleep(1000);
                }
            }, token);

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

            source.Cancel();
        }
    }
}
