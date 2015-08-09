

using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using MyContract;

namespace NetMessagingBindingWCFClient
{
    class Program
    {
        private static string sendKeyName = "SendAccessKeyMyQueue";
        private static string sendKeyValue = "P4OKDW7np6tWTcZ+MjubBcpeTZREtjSeNbP9daTMHXk=";

        private static string _queueName = "MyQueue";

        private static string ServiceNamespace = "johnsonwangnz";

        static void Main(string[] args)
        {
          MessageWithContext();
        }

        private static void MessageWithContext()
        {
            var transportClientEndpointBehavior = new TransportClientEndpointBehavior
            {
                TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(sendKeyName, sendKeyValue)
            };

            var serviceUri = ServiceBusEnvironment.CreateServiceUri("sb", ServiceNamespace, _queueName);

            var netMessagingBinding = new NetMessagingBinding
            {
                PrefetchCount = 10,
                TransportSettings = new NetMessagingTransportSettings
                {
                    // this is polling interval
                    BatchFlushInterval = TimeSpan.FromSeconds(0)
                }
            };

            var serviceEndpoint = new ServiceEndpoint(ContractDescription.GetContract(typeof(IRequest)),
                                                        netMessagingBinding,
                                                        new EndpointAddress(serviceUri));
            serviceEndpoint.Behaviors.Add(transportClientEndpointBehavior);
            var channelFactory = new ChannelFactory<IRequest>(serviceEndpoint);
            var channel = channelFactory.CreateChannel();
            Console.WriteLine("Press any key to send message...");
            Console.ReadKey();

            using (new OperationContextScope((IContextChannel)channel))
            {
                var brokeredMessageProperty = new BrokeredMessageProperty
                {
                    Label = "MyLabel",
                    MessageId = Guid.NewGuid().ToString()
                };
                brokeredMessageProperty.Properties.Add("Author", "Paolo Salvatori");
                brokeredMessageProperty.Properties.Add("Email", "paolos@microsoft.com");
                brokeredMessageProperty.Properties.Add("Country", "Italy");
                brokeredMessageProperty.Properties.Add("Priority", 1);
                OperationContext.Current.OutgoingMessageProperties.Add(BrokeredMessageProperty.Name, brokeredMessageProperty);
                channel.SendMessageWithContext("Hello from wcf client with context");
          
            } 
    
            Console.WriteLine("Message sent");
            Console.ReadKey();
        }

        private static void MessageOnly()
        {
            var transportClientEndpointBehavior = new TransportClientEndpointBehavior
            {
                TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(sendKeyName, sendKeyValue)
            };

            var serviceUri = ServiceBusEnvironment.CreateServiceUri("sb", ServiceNamespace, _queueName);

            var netMessagingBinding = new NetMessagingBinding
            {
                PrefetchCount = 10,
                TransportSettings = new NetMessagingTransportSettings
                {
                    BatchFlushInterval = TimeSpan.FromSeconds(0)
                }
            };

            var serviceEndpoint = new ServiceEndpoint(ContractDescription.GetContract(typeof(IRequest)),
                                                        netMessagingBinding,
                                                        new EndpointAddress(serviceUri));
            serviceEndpoint.Behaviors.Add(transportClientEndpointBehavior);
            var channelFactory = new ChannelFactory<IRequest>(serviceEndpoint);
            var channel = channelFactory.CreateChannel();
            Console.WriteLine("Press any key to send message...");
            Console.ReadKey();
            channel.SendMessage("Hello from wcf client");
            Console.WriteLine("Message sent");
            Console.ReadKey();
        }
    }
}
