using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace MyBehaviors
{
    /**
     * Called before send or after receive to inspect and replace message contents.
     * 
     * Instead of inspecting parameters, 
     * 
     * let's assume you want to inspect the messages flowing in and out of a service 
     * regardless of the operation. 
     * 
     * This is where you'd use the message inspection extensibility point. 
     * 
     * Unlike parameter inspection, the interfaces for message inspection are different 
     * for the dispatcher and proxy (IDispatchMessageInspector versus IClientMessageInspector). 
     * 
     * However, you can always implement both interfaces when you want to support both sides.
     * 
     * IDispatchMessageInspector has two methods: AfterReceiveRequest and BeforeSendReply, 
     * which means you get pre- and post-interception points for inspecting the WCF Message object. 
     * 
     * IClientMessageInspector also has two methods that provide the converse points: 
     * AfterReceiveReply and BeforeSendRequest.
     * 
     * **/
    public class ConsoleMessageTracer : IDispatchMessageInspector, IClientMessageInspector 
    { 
        private Message TraceMessage(MessageBuffer buffer) 
        { 
            Message msg = buffer.CreateMessage(); 
            Console.WriteLine("\n{0}\n", msg); 
            return buffer.CreateMessage(); 
        } 
        
        public object AfterReceiveRequest(ref Message request, IClientChannel channel,
            InstanceContext instanceContext) 
        {
            request = TraceMessage(request.CreateBufferedCopy(int.MaxValue)); 
            return null; 
        } 
        
        public void BeforeSendReply(ref Message reply, object correlationState) 
        { 
            reply = TraceMessage(reply.CreateBufferedCopy(int.MaxValue)); 
        } 
        
        public void AfterReceiveReply(ref Message reply, object correlationState) 
        { 
            reply = TraceMessage(reply.CreateBufferedCopy(int.MaxValue)); 
        } 
        
        public object BeforeSendRequest(ref Message request, IClientChannel channel) 
        { 
            request = TraceMessage(request.CreateBufferedCopy(int.MaxValue)); 
            return null; 
        } 
    }
}
