

using System;
using System.Text;
using System.Threading;
using Microsoft.ServiceBus.Messaging;

namespace Sender
{
    class Program
    {

        static string eventHubName = "myeventhub";
        static string connectionString = "Endpoint=sb://myeventhub-johnson.servicebus.windows.net/;SharedAccessKeyName=SendKeyMyEventHub;SharedAccessKey=7euXsVqb8FNeuRqsorw6rbZ2fii1F3FVh3AwxMwzNgU=";

        static void Main(string[] args)
        {
            Console.WriteLine("Press Ctrl-C to stop the sender process");
            Console.WriteLine("Press Enter to start now");
            Console.ReadLine();
            SendingRandomMessages();
        }

        static void SendingRandomMessages()
        {
            var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, eventHubName);
            while (true)
            {
                try
                {
                    var message = Guid.NewGuid().ToString();
                    Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, message);
                    eventHubClient.Send(new EventData(Encoding.UTF8.GetBytes(message)));
                }
                catch (Exception exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0} > Exception: {1}", DateTime.Now, exception.Message);
                    Console.ResetColor();
                }

                Thread.Sleep(200);
            }
        }
    }
}
