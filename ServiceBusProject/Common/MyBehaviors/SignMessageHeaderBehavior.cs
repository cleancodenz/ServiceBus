using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Security;
using System.Xml;

namespace MyBehaviors
{
    //Can not be used in configuration file, can only be used by code
    public class SignMessageHeaderBehavior : Attribute, IContractBehavior
    {

        string action;
        string header;
        string ns;

        public SignMessageHeaderBehavior(string header, string ns, string action)
        {
            this.header = header;
            this.ns = ns;
            this.action = action;
        }


        /**
         * The execution of this behavior comes rather late. 
         * Anyone that inspects the service description in the meantime, 
         * such as for metadata generation, won't see the protection level that we want to use.
         * 
         * One way of doing it is at when create HostFactory
         * 
         * ServiceEndpoint endpoint = host.Description.Endpoints.Find(typeof(IService));
         * OperationDescription operation = endpoint.Contract.Operations.Find("Action");
         * MessageDescription message = operation.Messages.Find("http://tempuri.org/IService/ActionResponse");
         * MessageHeaderDescription header = message.Headers[new XmlQualifiedName("aheader", "http://tempuri.org/")];
         * header.ProtectionLevel = ProtectionLevel.Sign;
         * 
         * **/
        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            ChannelProtectionRequirements requirements = bindingParameters.Find<ChannelProtectionRequirements>();
            XmlQualifiedName qName = new XmlQualifiedName(header, ns);
            MessagePartSpecification part = new MessagePartSpecification(qName);
            requirements.OutgoingSignatureParts.AddParts(part, action);
        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            
        }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
            
        }
    }
}
