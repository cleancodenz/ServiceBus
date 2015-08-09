
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
           // CreateQueueKeys();
           // CreateTopicAndSubscriptionsKeys();
            CreateEventHubKeys();
        }

        private static void CreateRelayKeys()
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

                Task.Run(() => nsManager.DeleteRelayAsync(relayDescription.Path)).Wait();
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

                var relay = Task.Run(() => nsManager.CreateRelayAsync(relayDescription)).Result;
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

        private static void CreateQueueKeys()
        {
            NamespaceManager nsManager = NamespaceManager.CreateFromConnectionString(
                "Endpoint=sb://johnsonwangnz.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=fBLL/4/+rEsCOiTQPNPS6DJQybykqE2HdVBsILrzMLY=");
        
            
            QueueDescription qd = new QueueDescription("MyQueue");

            //config queues

            //qd.MaxSizeInMegabytes = 5120;
            //qd.DefaultMessageTimeToLive = new TimeSpan(0, 1, 0);

            try
            {

                Task.Run(() => nsManager.DeleteQueueAsync("MyQueue")).Wait();
            }
            catch (AggregateException aex)
            {
                Console.WriteLine(aex.InnerException);
            }
            catch (MessagingException ex)
            {
                Console.WriteLine(ex);
            }


            var sendKeyMyQueue = SharedAccessAuthorizationRule.GenerateRandomKey();
            var sendKeyNameMyQueue = "SendAccessKeyMyQueue";
            var listenKeyMyQueue = SharedAccessAuthorizationRule.GenerateRandomKey();
            var listenKeyNameMyQueue = "ListenAccessKeyMyQueue";

            var manageKeyMyQueue = SharedAccessAuthorizationRule.GenerateRandomKey();
            var manageKeyNameMyQueue = "ManageAccessKeyMyQueue";
         
            qd.Authorization.Add(new SharedAccessAuthorizationRule(listenKeyNameMyQueue, listenKeyMyQueue,
                    new List<AccessRights> { AccessRights.Listen }));
            qd.Authorization.Add(new SharedAccessAuthorizationRule(sendKeyNameMyQueue, sendKeyMyQueue,
                    new List<AccessRights> { AccessRights.Send }));
            qd.Authorization.Add(new SharedAccessAuthorizationRule(manageKeyNameMyQueue, manageKeyMyQueue,
                    new List<AccessRights> { AccessRights.Manage, AccessRights.Send, AccessRights.Listen}));

            try
            {
                // normal scenario will be check existence first 
                var queue = Task.Run(() => nsManager.CreateQueueAsync(qd)).Result;
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
            new StreamWriter(@"MyQueueKeys.txt", true))
            {
                file.WriteLine("Queue Path:" + qd.Path);
                file.WriteLine("Listen Key Name:" + listenKeyNameMyQueue);
                file.WriteLine("Listen Key:" + listenKeyMyQueue);
                file.WriteLine("Send Key Name:" + sendKeyNameMyQueue);
                file.WriteLine("Send Key:" + sendKeyMyQueue);
                file.WriteLine("Manage Key Name:" + manageKeyNameMyQueue);
                file.WriteLine("Manage Key:" + manageKeyMyQueue);


            }

            Console.WriteLine("Listen access rule created with key '{0}' and name {2} on entity '{1}'", listenKeyMyQueue, qd.Path, listenKeyNameMyQueue);
            Console.WriteLine("Send access rule created with key '{0}' and name {2} on entity '{1}'", sendKeyMyQueue, qd.Path, sendKeyNameMyQueue);
            Console.WriteLine("Send access rule created with key '{0}' and name {2} on entity '{1}'", manageKeyMyQueue, qd.Path, manageKeyNameMyQueue);


            Console.ReadKey();
        }


        private static void CreateTopicAndSubscriptionsKeys()
        {
            const string topicName = "MyTopic";

            NamespaceManager nsManager = NamespaceManager.CreateFromConnectionString(
                "Endpoint=sb://johnsonwangnz.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=fBLL/4/+rEsCOiTQPNPS6DJQybykqE2HdVBsILrzMLY=");


            // Create topic 
            TopicDescription topicDescription = new TopicDescription(topicName);
            //config topics

            
            // td.MaxSizeInMegabytes = 5120;
            // td.DefaultMessageTimeToLive = new TimeSpan(0, 1, 0);


            // All messages, no filter
            SubscriptionDescription subscriptionDescription1 = new SubscriptionDescription(topicName,"MySubscription1");
            
            // receive messages with filter, the filter is on MessageNumber property
            SqlFilter highMessagesFilter = new SqlFilter("MessageNumber > 3");
            SubscriptionDescription subscriptionDescription2 = new SubscriptionDescription(topicName, "MySubscription2");

            if (nsManager.TopicExists(topicName))
            {
                Console.WriteLine("{0} topic already exists, deleting it...", topicName);
                // delete it 
                nsManager.DeleteTopic(topicName);
            }

            // Create keys for the topic and subscriptions

            var sendKey = SharedAccessAuthorizationRule.GenerateRandomKey();
            var sendKeyName = "SendAccessKeyMyTopic";
            var listenKeyForSubscription1 = SharedAccessAuthorizationRule.GenerateRandomKey();
            var listenKeyNameForSubscription1 = "ListenAccessKeyMySubscription1";

            var listenKeyForSubscription2 = SharedAccessAuthorizationRule.GenerateRandomKey();
            var listenKeyNameForSubscription2 = "ListenAccessKeyMySubscription2";

            var manageKey = SharedAccessAuthorizationRule.GenerateRandomKey();
            var manageKeyName = "ManageAccessKeyMyTopic";

            //Create credentials for topic, subscription does not have authorization, it is set here
            topicDescription.Authorization.Add(new SharedAccessAuthorizationRule(sendKeyName, sendKey,
                    new List<AccessRights> { AccessRights.Send }));
            topicDescription.Authorization.Add(new SharedAccessAuthorizationRule(manageKeyName, manageKey,
                    new List<AccessRights> { AccessRights.Manage, AccessRights.Send, AccessRights.Listen }));

            topicDescription.Authorization.Add(new SharedAccessAuthorizationRule(listenKeyNameForSubscription1, listenKeyForSubscription1,
                  new List<AccessRights> { AccessRights.Listen }));

            topicDescription.Authorization.Add(new SharedAccessAuthorizationRule(listenKeyNameForSubscription2, listenKeyForSubscription2,
                new List<AccessRights> { AccessRights.Listen }));

            try
            {
               
                //create topic

                nsManager.CreateTopic(topicDescription);

                //create subscriptions
                // all messages
                nsManager.CreateSubscription(subscriptionDescription1);
                // filtered messages
                nsManager.CreateSubscription(subscriptionDescription2, highMessagesFilter);

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
            new StreamWriter(@"MyTopicKeys.txt", true))
            {
                file.WriteLine("Topic Path:" + topicDescription.Path);
                file.WriteLine("Listen Key Name 1: " + listenKeyNameForSubscription1);
                file.WriteLine("Listen Key1: " + listenKeyForSubscription1);
                file.WriteLine("Listen Key Name 2: " + listenKeyNameForSubscription2);
                file.WriteLine("Listen Key2: " + listenKeyForSubscription2);
                file.WriteLine("Send Key Name:" + sendKeyName);
                file.WriteLine("Send Key:" + sendKey);
                file.WriteLine("Manage Key Name:" + manageKey);
                file.WriteLine("Manage Key:" + manageKey);


            }

            Console.WriteLine("Listen access rule created with key '{0}' and name {2} on entity '{1}'", listenKeyForSubscription1, topicDescription.Path, listenKeyNameForSubscription1);
            Console.WriteLine("Listen access rule created with key '{0}' and name {2} on entity '{1}'", listenKeyForSubscription2, topicDescription.Path, listenKeyNameForSubscription2);
            Console.WriteLine("Send access rule created with key '{0}' and name {2} on entity '{1}'", sendKey, topicDescription.Path, sendKeyName);
            Console.WriteLine("Send access rule created with key '{0}' and name {2} on entity '{1}'", manageKey, topicDescription.Path, manageKeyName);


            Console.ReadKey();
        }

       //this can not be done using code
        private static void CreateEventHubKeys()
        {

            const string eventHubName = "MyEventHub";
            NamespaceManager nsManager = NamespaceManager.CreateFromConnectionString(
                "Endpoint=sb://johnsonwangnz2.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=/wtH2nivH3W7ZKk3rSSwiid2zi98J+NYcMu52QGUqdM=");

            EventHubDescription eh = new EventHubDescription(eventHubName);


            if (nsManager.EventHubExists(eventHubName))
            {
                Console.WriteLine("{0} EventHub already exists, deleting it...", eventHubName);
                // delete it 
                nsManager.DeleteEventHub(eventHubName);

            }
     


            var sendKeyMyEventHub = SharedAccessAuthorizationRule.GenerateRandomKey();
            var sendKeyNameMyEventHub = "SendAccessKeyMyEventHub";
            var listenKeyMyEventHub = SharedAccessAuthorizationRule.GenerateRandomKey();
            var listenKeyNameMyEventHub = "ListenAccessKeyMyEventHub";

          

            eh.Authorization.Add(new SharedAccessAuthorizationRule(listenKeyNameMyEventHub, listenKeyMyEventHub,
                    new List<AccessRights> { AccessRights.Listen }));
            eh.Authorization.Add(new SharedAccessAuthorizationRule(sendKeyNameMyEventHub, sendKeyMyEventHub,
                    new List<AccessRights> { AccessRights.Send }));

            try
            {

                //create EventHub

                nsManager.CreateEventHub(eh);


            }
            catch (AggregateException aex)
            {
                Console.WriteLine(aex.InnerException);
            }
            catch (MessagingException ex)
            {
                Console.WriteLine(ex);
            }


            using (StreamWriter file =
            new StreamWriter(@"MyEventHubKeys.txt", true))
            {
                file.WriteLine("EventHub Path:" + eh.Path);
                file.WriteLine("Listen Key Name:" + listenKeyNameMyEventHub);
                file.WriteLine("Listen Key:" + listenKeyMyEventHub);
                file.WriteLine("Send Key Name:" + sendKeyNameMyEventHub);
                file.WriteLine("Send Key:" + sendKeyMyEventHub);
       
            }

            Console.WriteLine("Listen access rule created with key '{0}' and name {2} on entity '{1}'", listenKeyMyEventHub, eh.Path, listenKeyNameMyEventHub);
            Console.WriteLine("Send access rule created with key '{0}' and name {2} on entity '{1}'", sendKeyMyEventHub, eh.Path, sendKeyNameMyEventHub);
            
            Console.ReadKey();
        }
    }
}
