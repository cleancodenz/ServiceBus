

using System;

using MyContract;

namespace EchoServices
{
  
    public class EchoService : IEchoContract
    {
        public string Echo(string text)
        {
            Console.WriteLine("Echoing: {0}", text);
            return text;
        }
    }
}
