


using System;
using System.ServiceModel;
using Microsoft.ServiceBus;
using MyContract;

namespace EchoServices
{
    class Program
    {
        private static void Main(string[] args)
        {
            Two();
        }

        //NetTCP does not work because of tcp ports being blocked by firewall
        private static void One()
        {
            Console.WriteLine("Starting service...");
            ServiceHost sh = new ServiceHost(typeof(EchoService));

            sh.AddServiceEndpoint(
                typeof(IEchoContract), new NetTcpBinding(),
                "net.tcp://localhost:9358/solver");

            sh.AddServiceEndpoint(
                typeof(IEchoContract),
                new NetTcpRelayBinding(),
                ServiceBusEnvironment.CreateServiceUri("https", "johnsonwangnz", "solver"))
                .Behaviors.Add(new TransportClientEndpointBehavior
                {
                    TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("RootManageSharedAccessKey", "fBLL/4/+rEsCOiTQPNPS6DJQybykqE2HdVBsILrzMLY=")
                });

            sh.Open();
            Console.WriteLine("Listening...");
            Console.ReadLine();
            sh.Close();
        }

        private static void Two()
        {
             Console.WriteLine("Starting service...");
            
            // Configure the credentials through an endpoint behavior.
            TransportClientEndpointBehavior relayCredentials = new TransportClientEndpointBehavior();
            relayCredentials.TokenProvider =
              TokenProvider.CreateSharedAccessSignatureTokenProvider("RootManageSharedAccessKey", "fBLL/4/+rEsCOiTQPNPS6DJQybykqE2HdVBsILrzMLY=");

            // Create the binding with default settings.
            WS2007HttpRelayBinding binding = new WS2007HttpRelayBinding();

            // Get the service address.
            // Use the https scheme because by default the binding uses SSL for transport security.
            Uri address = ServiceBusEnvironment.CreateServiceUri("https", "johnsonwangnz", "MyService");

            // Create the service host.
            ServiceHost host = new ServiceHost(typeof(EchoService), address);
            // Add the service endpoint with the WS2007HttpRelayBinding.
            host.AddServiceEndpoint(typeof(IEchoContract), binding, address);
           
            // Add the credentials through the endpoint behavior.
            host.Description.Endpoints[0].Behaviors.Add(relayCredentials);
            
            // Start the service.
            host.Open();

            Console.WriteLine("Listening...");

            Console.ReadLine();
            host.Close();
        }
    }
}
