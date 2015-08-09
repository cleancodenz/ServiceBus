

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace QueueSample
{
    class Program
    {
        private static DataTable issues;
        private static List<BrokeredMessage> MessageList;

        private static string ServiceNamespace ="johnsonwangnz";
        private static string manageKeyName = "ManageAccessKeyMyQueue";
        private static string manageKeyValue = "+Hlo21ytEVKodG+AfdPGW+7cgvHFMxHDJHm6IDIkHsc=";

        private static string sendKeyName = "SendAccessKeyMyQueue";
        private static string sendKeyValue = "P4OKDW7np6tWTcZ+MjubBcpeTZREtjSeNbP9daTMHXk=";

        private static string listenKeyName = "ListenAccessKeyMyQueue";
        private static string listenKeyValue = "uap7GyIqDCazZhreZcuTX65xMOccwwMcQUU6Gd9t/Mk=";

     

        static void Main(string[] args)
        {
            issues = ParseCSVFile();
            MessageList = GenerateMessages(issues);
       
            SendMessages();

         //   Console.WriteLine("Now receiving messages from Queue.");
            
         //   ReceiveMessages();

          //  Console.WriteLine("All message received");

            Console.WriteLine("Now receiving messages by another app");
            
            Console.ReadKey();
        }


        //Queue and its credentials in another project, this never called
        static void CreateQueue()
        {
            // Create management credentials
            TokenProvider credentials = TokenProvider.CreateSharedAccessSignatureTokenProvider(manageKeyName, manageKeyValue);

            NamespaceManager namespaceClient = new NamespaceManager(ServiceBusEnvironment.CreateServiceUri("sb", ServiceNamespace, string.Empty), credentials);
            
            QueueDescription myQueue = namespaceClient.CreateQueue("MyQueue");
        }

        private static void SendMessages()
        {
            // Create management credentials
            TokenProvider credentials = TokenProvider.CreateSharedAccessSignatureTokenProvider(sendKeyName, sendKeyValue);

            MessagingFactory factory = MessagingFactory.Create(ServiceBusEnvironment.CreateServiceUri("sb", ServiceNamespace, string.Empty), credentials);

            QueueClient sendQueueClient = factory.CreateQueueClient("MyQueue");

            // Send messages
            Console.WriteLine("Now sending messages to the Queue.");
            for (int count = 0; count < 6; count++)
            {
                var issue = MessageList[count];
                issue.Label = issue.Properties["IssueTitle"].ToString();
                sendQueueClient.Send(issue);
                Console.WriteLine(string.Format("Message sent: {0}, {1}", issue.Label, issue.MessageId));
            }

            factory.Close();
            sendQueueClient.Close();

        }

        private static void ReceiveMessages()
        {
            // Create management credentials
            TokenProvider credentials = TokenProvider.CreateSharedAccessSignatureTokenProvider(listenKeyName, listenKeyValue);

            MessagingFactory factory = MessagingFactory.Create(ServiceBusEnvironment.CreateServiceUri("sb", ServiceNamespace, string.Empty), credentials);

            QueueClient receiveQueueClient = factory.CreateQueueClient("MyQueue");

            BrokeredMessage message;
            while ((message = receiveQueueClient.Receive(new TimeSpan(hours: 0, minutes: 1, seconds: 5))) != null)
            {
                Console.WriteLine(string.Format("Message received: {0}, {1}, {2}", message.SequenceNumber, message.Label, message.MessageId));
                message.Complete();

                Console.WriteLine("Processing message (sleeping...)");
                Thread.Sleep(1000);
            }

            factory.Close();

            receiveQueueClient.Close();

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

            // Iterate through the table and create a brokered message for each row
            foreach (DataRow item in issues.Rows)
            {
                BrokeredMessage message = new BrokeredMessage();
                foreach (DataColumn property in issues.Columns)
                {
                    message.Properties.Add(property.ColumnName, item[property]);
                }
                result.Add(message);
            }
            return result;
        }
    }
}
