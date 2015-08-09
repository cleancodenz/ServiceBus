


using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using CustomWCFComponents.MembershipProvider;
using CustomWCFComponents.RoleProvider;
using Microsoft.ServiceBus;
using MyContract;
using TestCertificates;

namespace EchoServices
{
    class Program
    {
        private static void Main(string[] args)
        {
           // Two();
           // Three();
            UserNameClient();
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

        //This is using root key which should be avoided in production 
        private static void Two()
        {
             Console.WriteLine("Starting service...");
            
            // Configure the credentials through an endpoint behavior.
            TransportClientEndpointBehavior relayCredentials = new TransportClientEndpointBehavior();
            relayCredentials.TokenProvider =
              TokenProvider.CreateSharedAccessSignatureTokenProvider("RootManageSharedAccessKey", "fBLL/4/+rEsCOiTQPNPS6DJQybykqE2HdVBsILrzMLY=");

            // Create the binding with default settings.
            WS2007HttpRelayBinding binding = new WS2007HttpRelayBinding();
            //binding.Security.Mode = EndToEndSecurityMode.Message;
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

        private static void Three()
        {
            Console.WriteLine("Starting service...");

            // Configure the credentials through an endpoint behavior.
            TransportClientEndpointBehavior relayCredentials = new TransportClientEndpointBehavior();
            relayCredentials.TokenProvider =
              TokenProvider.CreateSharedAccessSignatureTokenProvider("ListenAccessKeyMyService", "3VXc5wQwu479N/w2MaLtsk9fA7WWJsamsxtWcr8zbCY=");

            // Create the binding with default settings.
            WS2007HttpRelayBinding binding = new WS2007HttpRelayBinding();

           
            // it must be false to use previously generated relay endpoint and its special credentials
            binding.IsDynamic = false;
            //binding.Security.Mode = EndToEndSecurityMode.Message;
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
        private static void UserNameClient()
        {
            Console.WriteLine("Starting service...");

            // Configure the credentials through an endpoint behavior.
            TransportClientEndpointBehavior relayCredentials = new TransportClientEndpointBehavior();
            relayCredentials.TokenProvider =
              TokenProvider.CreateSharedAccessSignatureTokenProvider("ListenAccessKeyMyService", "3VXc5wQwu479N/w2MaLtsk9fA7WWJsamsxtWcr8zbCY=");

            // Create the binding with default settings.
            WS2007HttpRelayBinding binding = new WS2007HttpRelayBinding();

            // it must be false to use previously generated relay endpoint and its special credentials
            binding.IsDynamic = false;
           
            binding.Security.Mode = EndToEndSecurityMode.TransportWithMessageCredential;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
            binding.Security.Message.EstablishSecurityContext = false;
            binding.Security.Message.NegotiateServiceCredential = false;


            // Get the service address.
            // Use the https scheme because by default the binding uses SSL for transport security.
            Uri address = ServiceBusEnvironment.CreateServiceUri("https", "johnsonwangnz", "MyService");

            // Create the service host.
            ServiceHost host = new ServiceHost(typeof(EchoService), address);

            host.Credentials.ServiceCertificate.Certificate = MyCertificates.GetTestServiceCertificate();


            //Change user name authentication 
            host.Credentials.UserNameAuthentication.
                UserNamePasswordValidationMode = UserNamePasswordValidationMode.MembershipProvider;

            host.Credentials.UserNameAuthentication.MembershipProvider
                 = new MyMemberShipProvider();

            //set the authorization of user
            var serviceAuthroization = host.Authorization;
            serviceAuthroization.PrincipalPermissionMode = PrincipalPermissionMode.UseAspNetRoles;
            serviceAuthroization.RoleProvider = new MyRoleProvider();

            
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
