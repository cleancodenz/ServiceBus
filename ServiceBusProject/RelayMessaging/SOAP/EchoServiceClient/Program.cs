﻿

using System;
using System.ServiceModel;
using Microsoft.ServiceBus;
using MyContract;

namespace EchoServiceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            WS2007HttpRelayClientTwo();
        }


        private static void WS2007HttpRelayClientOne()
        {
            Console.WriteLine("Please enter any key to contact server...");
            Console.ReadLine();

            // Configure the credentials through an endpoint behavior.
            TransportClientEndpointBehavior relayCredentials = new TransportClientEndpointBehavior();
            relayCredentials.TokenProvider =
              TokenProvider.CreateSharedAccessSignatureTokenProvider("RootManageSharedAccessKey", "fBLL/4/+rEsCOiTQPNPS6DJQybykqE2HdVBsILrzMLY=");


            // Get the service address.
            // Use the https scheme because by default the binding uses SSL for transport security.
            Uri address = ServiceBusEnvironment.CreateServiceUri("https", "johnsonwangnz", "MyService");

            // Create the binding with default settings.
            WS2007HttpRelayBinding binding = new WS2007HttpRelayBinding();

            // Create a channel factory for the specified channel type.
            // This channel factory is used to create client channels to the service. 
            // Each client channel the channel factory creates is configured to use the 
            // WS2007HttpRelayBinding that is passed to the constructor of the channel. 
            var channelFactory = new ChannelFactory<IEchoContract>(
                binding, new EndpointAddress(address));

            channelFactory.Endpoint.Behaviors.Add(relayCredentials);

            // Create and open the client channel.
            IEchoContract echoService = channelFactory.CreateChannel();

            var result = echoService.Echo("HelloWorld!");

            Console.WriteLine(result);

            Console.ReadLine();


        }

        private static void WS2007HttpRelayClientTwo()
        {
            Console.WriteLine("Please enter any key to contact server...");
            Console.ReadLine();


            var cf = new ChannelFactory<IEchoContract>(
                new WS2007HttpRelayBinding(),
                new EndpointAddress(ServiceBusEnvironment.CreateServiceUri("https", "johnsonwangnz", "MyService")));

            cf.Endpoint.Behaviors.Add(
                new TransportClientEndpointBehavior
                {
                    TokenProvider =
                    TokenProvider.CreateSharedAccessSignatureTokenProvider("RootManageSharedAccessKey", "fBLL/4/+rEsCOiTQPNPS6DJQybykqE2HdVBsILrzMLY=")
                });

            var ch = cf.CreateChannel();
            var result = ch.Echo("HelloWorld!");
            Console.WriteLine(result);
      
            Console.ReadLine();



        }
    }
}
