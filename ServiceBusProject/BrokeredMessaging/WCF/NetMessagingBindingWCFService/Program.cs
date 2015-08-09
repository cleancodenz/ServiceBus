

using System;
using System.ServiceModel;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using MyContract;

namespace NetMessagingBindingWCFService
{
    class Program
    {
        private static string listenKeyName = "ListenAccessKeyMyQueue";
        private static string listenKeyValue = "uap7GyIqDCazZhreZcuTX65xMOccwwMcQUU6Gd9t/Mk=";

        private static string _queueName = "MyQueue";

        private static string ServiceNamespace = "johnsonwangnz";

        static void Main(string[] args)
        {
            var transportClientEndpointBehavior = new TransportClientEndpointBehavior
            {
                TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(listenKeyName, listenKeyValue)
            };
            
            var serviceUri = ServiceBusEnvironment.CreateServiceUri("sb", ServiceNamespace, _queueName);

            // Instantiate the host with the contract and URI. 
            var host = new ServiceHost(typeof(RequestService), serviceUri);


            var netMessagingBinding = new NetMessagingBinding
            {
                PrefetchCount = 10,
                TransportSettings = new NetMessagingTransportSettings
                {
                    // this is polling interval
                    BatchFlushInterval = TimeSpan.FromSeconds(0)
                }
            };

            // Add the service endpoints to the service host 
            host.AddServiceEndpoint(typeof(IRequest), netMessagingBinding, string.Empty);

            Console.WriteLine("Listening... ");
            Console.Write(" - ");
            Console.WriteLine(host.Description.Endpoints[0].Address.Uri.AbsoluteUri);

            foreach (var endpoint in host.Description.Endpoints)
            {
                endpoint.Behaviors.Add(transportClientEndpointBehavior);
            }

            host.Open();

            Console.ReadLine();

        }
    }
}
