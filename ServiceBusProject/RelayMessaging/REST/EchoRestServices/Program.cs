

using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using Microsoft.ServiceBus;
using MyContract;

namespace EchoRestServices
{
    class Program
    {
        static void Main(string[] args)
        {
            Two();
        }
        private static void Two()
        {
            Console.WriteLine("Starting service...");

            // Configure the credentials through an endpoint behavior.
            TransportClientEndpointBehavior relayCredentials = new TransportClientEndpointBehavior();
            relayCredentials.TokenProvider =
              TokenProvider.CreateSharedAccessSignatureTokenProvider("RootManageSharedAccessKey", "fBLL/4/+rEsCOiTQPNPS6DJQybykqE2HdVBsILrzMLY=");

            // Create the binding with default settings.
            WebHttpRelayBinding binding = new WebHttpRelayBinding();

            binding.Security.RelayClientAuthenticationType = RelayClientAuthenticationType.None;
            // Get the service address.
            // Use the https scheme because by default the binding uses SSL for transport security.
            Uri address = ServiceBusEnvironment.CreateServiceUri("https", "johnsonwangnz", "Rest");

            // Create the web service host.
            WebServiceHost host = new WebServiceHost(typeof(EchoRestService), address);
            // Add the service endpoint with the WS2007HttpRelayBinding.
            host.AddServiceEndpoint(typeof(IEchoRestContract), binding, address);

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
