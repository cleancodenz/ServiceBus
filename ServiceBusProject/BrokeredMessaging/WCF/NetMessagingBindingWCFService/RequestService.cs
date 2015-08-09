

using System;
using System.ServiceModel;
using Microsoft.ServiceBus.Messaging;
using MyContract;
using System.ServiceModel.Channels;

namespace NetMessagingBindingWCFService
{
   public class RequestService : IRequest
    {
       public void SendMessage(string message)
       {
          Console.WriteLine("Message received by service: "+ message);
       }


       public void SendMessageWithContext(string message)
       {
           Console.WriteLine("Message received by service: " + message);
           // Get the message properties 
           var incomingProperties = OperationContext.Current.IncomingMessageProperties;
           var brokeredMessageProperty = incomingProperties[BrokeredMessageProperty.Name] as BrokeredMessageProperty; 

           if (brokeredMessageProperty != null)
           {
               foreach (var property in brokeredMessageProperty.Properties)
               {
                   Console.WriteLine(string.Format("Property {0} Value {1}",property.Key,property.Value));
               }
           }

           //Removing the message 
           ReceiveContext receiveContext;
           if (ReceiveContext.TryGet(incomingProperties, out receiveContext))
           {
               receiveContext.Complete(TimeSpan.FromSeconds(10.0d));
           }
           else
           {
               throw new InvalidOperationException("Receiver is in peek lock mode but receive context is not available!");
           } 
   
       }
    }
}
