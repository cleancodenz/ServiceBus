using System;
using System.Net;
using System.ServiceModel;


namespace EchoRestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            WS2007HttpRelayClientTwo();
        }

        private static void WS2007HttpRelayClientTwo()
        {
            Console.WriteLine("Please enter any key to contact server...");
            Console.ReadLine();

            using (var client = new WebClient())
            {
                var bytes = client.DownloadData("https://johnsonwangnz.servicebus.windows.net/Rest/EchoText/HelloWorld");

               var result = System.Text.Encoding.Default.GetString(bytes);
                
               Console.WriteLine(result);
            }

            Console.ReadLine();



        }
    }
}
