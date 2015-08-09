

using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security.Tokens;
using MyBehaviors;

namespace CustomWCFComponents.CustomBindings
{
    public class UserNameCertificateBinding : CustomBinding
    {
        public UserNameCertificateBinding()
        {
            //add  security
           // var securityElement =
            //   SecurityBindingElement.CreateUserNameForCertificateBindingElement();

            var securityElement = new SymmetricSecurityBindingElement();

            var x509TokenParameters = new X509SecurityTokenParameters();
            // how to find certificate
            // this will be used by securitymanager to find the certificate when create x509security tokens
            //x509TokenParameters.X509ReferenceStyle = X509KeyIdentifierClauseType.Thumbprint;

            //The token is never included in messages but is referenced. The token must be known to the recipient out of band
            x509TokenParameters.InclusionMode = SecurityTokenInclusionMode.Never;
            securityElement.ProtectionTokenParameters = x509TokenParameters;
            
            securityElement.EndpointSupportingTokenParameters.
                SignedEncrypted.Add(new UserNameSecurityTokenParameters());
            

            securityElement.MessageSecurityVersion = MessageSecurityVersion.WSSecurity11WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11;
            
            securityElement.RequireSignatureConfirmation = true;


            Elements.Add(securityElement);

            // Message Encoding
            var textEncoding = new GZipMessageEncodingBindingElement();
            textEncoding.MessageVersion = MessageVersion.Soap12WSAddressing10;
            Elements.Add(textEncoding);

            // Transport
            Elements.Add(new HttpTransportBindingElement());
        }
    }
}
