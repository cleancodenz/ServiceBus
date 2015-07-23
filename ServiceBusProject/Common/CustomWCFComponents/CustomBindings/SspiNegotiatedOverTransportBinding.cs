

using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;

namespace CustomWCFComponents.CustomBindings
{
    /**
     * Need https
     * 
     * The WS-Trust is able to be produced in service consumption
     * 
     * With this mode a negotiation protocol is used to perform client and server authentication. Kerberos is used if possible, otherwise NTLM.
     * The way it has been proved is using anonymous
     * 
     * Client
     * 
     *    clientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Anonymous;
     * 
     * Service
     * 
     *      host.Credentials.WindowsAuthentication.AllowAnonymousLogons = true;
     * 
     * **/
    public class SspiNegotiatedOverTransportBinding : CustomBinding
    {
        
        public SspiNegotiatedOverTransportBinding()
        {
            //add  security
            var securityElement =
                SecurityBindingElement.CreateSspiNegotiationOverTransportBindingElement();

            // Add login

        
            Elements.Add(securityElement);

            // Message Encoding
            var textEncoding = new TextMessageEncodingBindingElement();
            textEncoding.MessageVersion = MessageVersion.Soap12WSAddressing10;
            Elements.Add(textEncoding);

            // Transport
            var transport = new HttpsTransportBindingElement();
            Elements.Add(transport);

        }
    }
}
