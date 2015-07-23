

using System;
using System.Globalization;
using System.ServiceModel;

namespace EchoServices
{
    class Program
    {
        static void Main(string[] args)
        {
            /**
            // Create the binding with default settings.
            NetTcpRelayBinding binding = new NetTcpRelayBinding();

            // Get the service URI.
            Uri serviceAddress = ServiceBusEnvironment.CreateServiceUri("sb", serviceNamespace,
                   String.Format(CultureInfo.InvariantCulture, "{0}/MyService/", session));

            // Create the service host.
            ServiceHost host = new ServiceHost(typeof(MyService), serviceAddress);
            // Add the service endpoint with the NetTcpRelayBinding binding.
            host.AddServiceEndpoint(typeof(IMyContract), binding, serviceAddress);
            // Add the credentials through the endpoint behavior.
            host.Description.Endpoints[0].Behaviors.Add(relayCredentials);
            // Start the service.
            host.Open();

            // Create a channel factory for the specified channel type.
            // This channel factory is used to create client channels to the service. 
            // Each client channel the channel factory creates is configured to use the 
            // NetTcpRelayBinding that is passed to the constructor of the channel factory.
            ChannelFactory<IMyChannel> channelFactory = new ChannelFactory<IMyChannel>(
                binding, new EndpointAddress(serviceAddress));
            channelFactory.Endpoint.Behaviors.Add(relayCredentials);

            // Create and open the client channel.
            IMyChannel channel = channelFactory.CreateChannel();
            channel.Open();
             * */
        }
    }
}
