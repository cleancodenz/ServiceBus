

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace TopicAndSubscriptions
{
    class Program
    {
        private static string ServiceNamespace = "johnsonwangnz";
        private static DataTable issues;
        private static List<BrokeredMessage> MessageList;

        private static string myTopicName = "MyTopic";

        private static string sendKeyName = "SendAccessKeyMyTopic";
        private static string sendKeyValue = "GUwbEzeoKyg2O1ood3gPmVS9PGN0/RWQ/hkG+hh7038=";


        private static string listenKeyMySubscription1Name = "ListenAccessKeyMySubscription1";
        private static string listenKeyMySubscription1Value = "rOICcGdAfksjhWDHQODyAuuTC+kTdpleeY/wbjV8y3Y=";

        private static string listenKeyMySubscription2Name = "ListenAccessKeyMySubscription2";
        private static string listenKeyMySubscription2Value = "KK4WLnLMcodh01w2VZ/wgxKLW1Hk1aoNAyFFksc+KcM=";


        static void Main(string[] args)
        {
            issues = ParseCSVFile();
            MessageList = GenerateMessages(issues);
            SendMessages();
            
            Console.WriteLine("Now receiving messages from subscription1");

            ReceiveMessages("MySubscription1");
            
            Console.WriteLine("Now receiving messages from subscription2");

            ReceiveMessages("MySubscription2");

            Console.WriteLine("All message received");

           
            Console.ReadKey();
        }

        private static void SendMessages()
        {
            // Create management credentials
            TokenProvider credentials = TokenProvider.CreateSharedAccessSignatureTokenProvider(sendKeyName, sendKeyValue);

            MessagingFactory factory = MessagingFactory.Create(ServiceBusEnvironment.CreateServiceUri("sb", ServiceNamespace, string.Empty), credentials);

            MessageSender sender = factory.CreateMessageSender(myTopicName);

         
            // Send messages
            Console.WriteLine("Now sending messages to the Topic.");
            for (int count = 0; count < 6; count++)
            {
                var issue = MessageList[count];
                issue.Label = issue.Properties["IssueTitle"].ToString();
                sender.Send(issue);
                Console.WriteLine(string.Format("Message sent: {0}, {1}", issue.Label, issue.MessageId));
            }

            factory.Close();
        

        }

     
        private static void ReceiveMessages(string subscriptionName)
        {
            // Create management credentials
            TokenProvider credentials = TokenProvider.CreateSharedAccessSignatureTokenProvider(listenKeyMySubscription1Name, listenKeyMySubscription1Value);

            MessagingFactory factory = MessagingFactory.Create(ServiceBusEnvironment.CreateServiceUri("sb", ServiceNamespace, string.Empty), credentials);

            MessageReceiver receiver = factory.CreateMessageReceiver(myTopicName + "/subscriptions/" + subscriptionName);

          
            BrokeredMessage message;
            while ((message = receiver.Receive(new TimeSpan(hours: 0, minutes: 1, seconds: 5))) != null)
            {
                try
                {
                    Console.WriteLine(string.Format("Message received: {0}, {1}, {2}", message.SequenceNumber, message.Label, message.MessageId));
                    message.Complete();

                    Console.WriteLine("Processing message (sleeping...)");
                    Thread.Sleep(1000);
                }
                catch (Exception)
                {

                    message.Abandon();
                }
             
            }

            factory.Close();

         

        }

        static DataTable ParseCSVFile()
        {
            DataTable tableIssues = new DataTable("Issues");
            string path = @"..\..\data.csv";
            try
            {
                using (StreamReader readFile = new StreamReader(path))
                {
                    string line;
                    string[] row;

                    // create the columns
                    line = readFile.ReadLine();
                    foreach (string columnTitle in line.Split(','))
                    {
                        tableIssues.Columns.Add(columnTitle);
                    }

                    while ((line = readFile.ReadLine()) != null)
                    {
                        row = line.Split(',');
                        tableIssues.Rows.Add(row);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.ToString());
            }

            return tableIssues;
        }

        static List<BrokeredMessage> GenerateMessages(DataTable issues)
        {
            // Instantiate the brokered list object
            List<BrokeredMessage> result = new List<BrokeredMessage>();
            var i = 0;
            // Iterate through the table and create a brokered message for each row
            foreach (DataRow item in issues.Rows)
            {
                BrokeredMessage message = new BrokeredMessage();

                // add a MessageNumber property for filters
                message.Properties.Add("MessageNumber", i);
                
                foreach (DataColumn property in issues.Columns)
                {
                    
                    message.Properties.Add(property.ColumnName, item[property]);
                }
                result.Add(message);
                i++;
            }
            return result;
        }
    }
}
