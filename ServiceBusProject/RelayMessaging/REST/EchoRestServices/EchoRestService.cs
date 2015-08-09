

using System;
using System.ServiceModel.Web;
using MyContract;

namespace EchoRestServices
{
    public class EchoRestService :IEchoRestContract
    {
        [WebGet(UriTemplate = "EchoText/{text}",
               RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json)]
        public string Echo(string text)
        {
            Console.WriteLine("Echoing: {0}", text);
            return text;
        }



        [WebGet(UriTemplate = "SimpleText", 
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json
           )]
        public string SimpleText()
        {
            Console.WriteLine("SimpleText method is called");
            return "Hello World!";
        }
    }
}
