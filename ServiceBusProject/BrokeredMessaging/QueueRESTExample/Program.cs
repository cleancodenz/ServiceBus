

using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace QueueRESTExample
{
    class Program
    {
        private static string baseAddress = "https://johnsonwangnz.servicebus.windows.net/";

        private static string sendKeyName = "SendAccessKeyMyQueue";
        private static string sendKeyValue = "P4OKDW7np6tWTcZ+MjubBcpeTZREtjSeNbP9daTMHXk=";

        private static string listenKeyName = "ListenAccessKeyMyQueue";
        private static string listenKeyValue = "uap7GyIqDCazZhreZcuTX65xMOccwwMcQUU6Gd9t/Mk=";

        // this needs permission on namespace
        private static string manageKeyName = "RootManageSharedAccessKey";
        private static string manageKeyValue = "fBLL/4/+rEsCOiTQPNPS6DJQybykqE2HdVBsILrzMLY=";

        private static string _queueName = "MyQueue";

        static void Main(string[] args)
        {
            // send and receive from queue
            var sendToken = GetSASToken(sendKeyName, sendKeyValue);
            SendMessage(_queueName, "msg1", sendToken);
            var receiveToken = GetSASToken(listenKeyName, listenKeyValue);
            string msg = ReceiveAndDeleteMessage(_queueName, receiveToken);

            // manage resources

            var manageToken = GetSASToken(manageKeyName, manageKeyValue);
            // Get an Atom feed with all the queues in the namespace
            // do not delete queues
            Console.WriteLine(GetResources("$Resources/Queues", manageToken));

            Console.ReadKey();

        }

        private static string GetSASToken(string SASKeyName, string SASKeyValue)
        {
            TimeSpan fromEpochStart = DateTime.UtcNow - new DateTime(1970, 1, 1);
            string expiry = Convert.ToString((int)fromEpochStart.TotalSeconds + 3600);
            string stringToSign = WebUtility.UrlEncode(baseAddress) + "\n" + expiry;
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(SASKeyValue));

            string signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
            string sasToken = String.Format(CultureInfo.InvariantCulture, "SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}",
                WebUtility.UrlEncode(baseAddress), WebUtility.UrlEncode(signature), expiry, SASKeyName);
            return sasToken;
        }

        // Sends a message to the "queueName" queue, given the name and the value to enqueue
        // Uses an HTTP POST request.
        private static void SendMessage(string queueName, string body, string token)
        {
            string fullAddress = baseAddress + queueName + "/messages" + "?timeout=60&api-version=2013-08 ";
            Console.WriteLine("\nSending message {0} - to address {1}", body, fullAddress);
            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.Authorization] = token;

            // Add brokered message properties “TimeToLive” and “Label”.
            webClient.Headers.Add("BrokerProperties", "{ \"TimeToLive\":30, \"Label\":\"M1\"}");

            // Add custom properties “Priority” and “Customer”.
            webClient.Headers.Add("Priority", "High");
            webClient.Headers.Add("Customer", "12345");

            webClient.UploadData(fullAddress, "POST", Encoding.UTF8.GetBytes(body));
        }

        // Receives and deletes the next message from the given resource (queue, topic, or subscription)
        // using the resourceName and an HTTP DELETE request
        private static string ReceiveAndDeleteMessage(string resourceName, string token)
        {
            string fullAddress = baseAddress + resourceName + "/messages/head" + "?timeout=60";
            Console.WriteLine("\nRetrieving message from {0}", fullAddress);
            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.Authorization] = token;

            byte[] response = webClient.UploadData(fullAddress, "DELETE", new byte[0]);
            string responseStr = Encoding.UTF8.GetString(response);

            Console.WriteLine(responseStr);
            return responseStr;
        }


        private static string GetResources(string resourceAddress, string token)
        {
            string fullAddress = baseAddress + resourceAddress;
            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.Authorization] = token;
            Console.WriteLine("\nGetting resources from {0}", fullAddress);
            return FormatXml(webClient.DownloadString(fullAddress));
        }

        private static string DeleteResource(string resourceName, string token)
        {
            string fullAddress = baseAddress + resourceName;
            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.Authorization] = token;

            Console.WriteLine("\nDeleting resource at {0}", fullAddress);
            byte[] response = webClient.UploadData(fullAddress, "DELETE", new byte[0]);
            return Encoding.UTF8.GetString(response);
        }
        // Formats the XML string to be more human-readable; intended for display purposes
        private static string FormatXml(string inputXml)
        {
            XmlDocument document = new XmlDocument();
            document.Load(new StringReader(inputXml));

            StringBuilder builder = new StringBuilder();
            using (XmlTextWriter writer = new XmlTextWriter(new StringWriter(builder)))
            {
                writer.Formatting = Formatting.Indented;
                document.Save(writer);
            }

            return builder.ToString();
        }
    }
}
