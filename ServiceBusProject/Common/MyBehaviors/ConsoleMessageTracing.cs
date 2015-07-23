using System;

using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;


namespace MyBehaviors
{
    public class ConsoleMessageTracing : Attribute, 
        IEndpointBehavior, IServiceBehavior 
    
    { 
        void IEndpointBehavior.ApplyClientBehavior(ServiceEndpoint endpoint, 
            ClientRuntime clientRuntime) 
        { 
            clientRuntime.MessageInspectors.Add(new ConsoleMessageTracer());
        } 
        
        void IEndpointBehavior.ApplyDispatchBehavior(ServiceEndpoint endpoint, 
            EndpointDispatcher endpointDispatcher) 
        { 
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add( new ConsoleMessageTracer()); 
        } // remaining methods empty 
        
        void IServiceBehavior.ApplyDispatchBehavior( ServiceDescription desc, 
            ServiceHostBase host) 
        { 
            foreach ( ChannelDispatcher cDispatcher in host.ChannelDispatchers) 
                foreach (EndpointDispatcher eDispatcher in cDispatcher.Endpoints) 
                    eDispatcher.DispatchRuntime.MessageInspectors.Add( new ConsoleMessageTracer()); 
        } // remaining methods empty 

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            
        }



        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            
        }
    }
}
