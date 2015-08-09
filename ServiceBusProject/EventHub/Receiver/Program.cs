

using System;
using Microsoft.ServiceBus.Messaging;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
          

           string eventHubName = "myeventhub";
           string eventHubConnectionString =
                "Endpoint=sb://myeventhub-johnson.servicebus.windows.net/;SharedAccessKeyName=ListenKeyMyEventHub;SharedAccessKey=bdIVbIb/zpblWEXtTVRWs0wfL/tPKyMFyPWI9+9PFP8=";


            string storageAccountName = "jwwas";
            string storageAccountKey = "cKBEo0jEgIDq7qApSr0KRNP+lD+yOylDomrpfI9CNrhfAeQwW0iE+/UAlX9A7sAcFn5Phm3muu9Qs2igBqRNfA==";
            string storageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
                storageAccountName, storageAccountKey);

            string eventProcessorHostName = Guid.NewGuid().ToString();
            EventProcessorHost eventProcessorHost = new EventProcessorHost(eventProcessorHostName, eventHubName, EventHubConsumerGroup.DefaultGroupName, eventHubConnectionString, storageConnectionString);
            Console.WriteLine("Registering EventProcessor...");
            eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>().Wait();

            Console.WriteLine("Receiving. Press enter key to stop worker.");
            Console.ReadLine();
            eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }
    }
}
