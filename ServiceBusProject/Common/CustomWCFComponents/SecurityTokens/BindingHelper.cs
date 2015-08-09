

using System.ServiceModel.Channels;
using System.ServiceModel.Security.Tokens;

namespace CustomWCFComponents.SecurityTokens
{
    public static class BindingHelper
    {
        public static Binding CreateCreditCardBinding()
        {
            HttpTransportBindingElement httpTransport = new HttpTransportBindingElement();

            
            // The message security binding element is configured to require a credit card
            // token that is encrypted with the service's certificate. 
            SymmetricSecurityBindingElement messageSecurity = new SymmetricSecurityBindingElement();
            messageSecurity.EndpointSupportingTokenParameters.SignedEncrypted.Add(new CreditCardTokenParameters());
            X509SecurityTokenParameters x509ProtectionParameters = new X509SecurityTokenParameters();
            x509ProtectionParameters.InclusionMode = SecurityTokenInclusionMode.Never;
            messageSecurity.ProtectionTokenParameters = x509ProtectionParameters;
            return new CustomBinding(messageSecurity, httpTransport);
        }
    }
}
