

using System;
using System.ServiceModel.Web;
using MyContract;

namespace EchoRestServices
{
    public class EchoRestService :IEchoContract
    {
        [WebGet]
        public string Echo(string text)
        {
            Console.WriteLine("Echoing: {0}", text);
            return text;
        }
    }
}
