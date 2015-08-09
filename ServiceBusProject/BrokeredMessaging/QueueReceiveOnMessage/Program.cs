

using System;
using Microsoft.ServiceBus.Messaging;

namespace QueueReceiveOnMessage
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start");
            Console.ReadKey();

            string connectionString = "Endpoint=sb://johnsonwangnz.servicebus.windows.net/;SharedAccessKeyName=ListenAccessKeyMyQueue;SharedAccessKey=uap7GyIqDCazZhreZcuTX65xMOccwwMcQUU6Gd9t/Mk=";
            QueueClient Client =
              QueueClient.CreateFromConnectionString(connectionString, "MyQueue");

            // Configure the callback options
            OnMessageOptions options = new OnMessageOptions();
            options.AutoComplete = false;
            options.AutoRenewTimeout = TimeSpan.FromMinutes(1);

            // Callback to handle received messages
            Client.OnMessage((message) =>
            {
                try
                {
                    // Process message from queue
                    Console.WriteLine("Body: " + message.GetBody<string>());
                    Console.WriteLine("MessageID: " + message.MessageId);
                    Console.WriteLine("Message Label " + message.Label);

                    // Remove message from queue
                    message.Complete();
                }
                catch (Exception)
                {
                    // Indicates a problem, unlock message in queue
                    message.Abandon();
                }
            }, options);

            Console.WriteLine("Press any key to finish");
            Console.ReadKey();
        }
    }
}
