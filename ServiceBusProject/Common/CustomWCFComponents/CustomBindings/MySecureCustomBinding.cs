using System.ServiceModel;
using System.ServiceModel.Channels;

namespace CustomWCFComponents.CustomBindings
{
    public class MySecureCustomBinding : CustomBinding
    {
        //Assimulating wshttpbinding as BindingElementCollection CreateBindingElements()

        public MySecureCustomBinding() : base()
        {
            // TransactionFlow, 
            Elements.Add(GetDefaultTransactionFlowBindingElement());

            // not sure about reliable session
            
            // Security
            Elements.Add(SecureConversionCertificate());

            // Message Encoding
            var textEncoding = new TextMessageEncodingBindingElement();
            textEncoding.MessageVersion = MessageVersion.Soap12WSAddressing10;
            Elements.Add(textEncoding);

            // Transport

            Elements.Add(new HttpTransportBindingElement());
        }

        private TransactionFlowBindingElement GetDefaultTransactionFlowBindingElement()
        {
            TransactionFlowBindingElement tfbe = new TransactionFlowBindingElement();
            tfbe.TransactionProtocol = TransactionProtocol.WSAtomicTransactionOctober2004;
            return tfbe;
        }

        private SecurityBindingElement AnonymousForCertificate()
        {
            var securityElement = 
                SecurityBindingElement.CreateAnonymousForCertificateBindingElement();

            return securityElement;
        }

        // This is simulating wshttpbinding message security clientcredential none 
        private SecurityBindingElement SecureConversionCertificate()
        {
            // boot strap security 
            var bootstrapSecurityElement =
                SecurityBindingElement.CreateSslNegotiationBindingElement(false, true);
            
            var securityElement =
            SecurityBindingElement.CreateSecureConversationBindingElement(bootstrapSecurityElement, true);
            return securityElement;

        }
    }
}
