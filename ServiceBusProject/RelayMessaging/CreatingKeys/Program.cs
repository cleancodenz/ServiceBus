
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace CreatingKeys
{
   // Everytime it is run a pair of keys will be created

    class Program
    {
        static void Main(string[] args)
        {
            NamespaceManager nsManager = NamespaceManager.CreateFromConnectionString(
                "Endpoint=sb://johnsonwangnz.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=fBLL/4/+rEsCOiTQPNPS6DJQybykqE2HdVBsILrzMLY=");
            var relayDescription = new RelayDescription("MyService", RelayType.Http)
            {
                RequiresClientAuthorization = true,
                RequiresTransportSecurity = true
            };
            
            try
            {
               
               Task.Run(()=>nsManager.DeleteRelayAsync(relayDescription.Path)).Wait();
            }
            catch (AggregateException aex)
            {
                Console.WriteLine(aex.InnerException);
            }
            catch (MessagingException ex)
            {
                Console.WriteLine(ex);
            }
            

            var sendKeyMyService = SharedAccessAuthorizationRule.GenerateRandomKey();
            var sendKeyNameMyService = "SendAccessKeyMyService";
            var listenKeyMyService = SharedAccessAuthorizationRule.GenerateRandomKey();
            var listenKeyNameMyService = "ListenAccessKeyMyService";

            relayDescription.Authorization.Add(new SharedAccessAuthorizationRule(listenKeyNameMyService, listenKeyMyService,
                    new List<AccessRights> { AccessRights.Listen }));
            relayDescription.Authorization.Add(new SharedAccessAuthorizationRule(sendKeyNameMyService, sendKeyMyService,
                    new List<AccessRights> { AccessRights.Send }));

            try
            {
               
                var relay = Task.Run(()=>nsManager.CreateRelayAsync(relayDescription)).Result;
            }
            catch (AggregateException aex)
            {
                Console.WriteLine(aex.InnerException);
            }
            catch (MessagingException ex)
            {
                Console.WriteLine(ex);
            }

            // the relay is available from azure portal, but not the keys

            using (StreamWriter file =
            new StreamWriter(@"MyServiceKeys.txt", true))
            {
                file.WriteLine("Service Path:" + relayDescription.Path);
                file.WriteLine("Listen Key Name:" + listenKeyNameMyService);
                file.WriteLine("Listen Key:" + listenKeyMyService);
                file.WriteLine("Send Key Name:" + sendKeyNameMyService);
                file.WriteLine("Send Key:" + sendKeyMyService);

            }

            Console.WriteLine("Listen access rule created with key '{0}' and name {2} on entity '{1}'", listenKeyMyService, relayDescription.Path, listenKeyNameMyService);
            Console.WriteLine("Send access rule created with key '{0}' and name {2} on entity '{1}'", sendKeyMyService, relayDescription.Path, sendKeyNameMyService);

            Console.ReadKey();
        }
    }
}
