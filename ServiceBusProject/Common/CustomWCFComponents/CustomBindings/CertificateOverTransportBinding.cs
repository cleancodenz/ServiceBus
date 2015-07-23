
using System.ServiceModel.Channels;

namespace CustomWCFComponents.CustomBindings
{
    /**
     * Need https
     * 
     *  WS-Security request header
     *  
     * <o:Security s:mustUnderstand="1" xmlns:o="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd">
     *      <u:Timestamp u:Id="_0">
     *          <u:Created>2015-03-09T01:02:09.837Z</u:Created>
     *          <u:Expires>2015-03-09T01:07:09.837Z</u:Expires>
     *      </u:Timestamp>
     *      <o:BinarySecurityToken u:Id="uuid-5a2be53c-cd9d-42cf-8075-a02f029bf72e-1" ValueType="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3">
     *          MIIDETCCAf2gAwIB...
     *      </o:BinarySecurityToken>
     *      <Signature xmlns="http://www.w3.org/2000/09/xmldsig#">
     *          <SignedInfo>
     *              <CanonicalizationMethod Algorithm="http://www.w3.org/2001/10/xml-exc-c14n#"/>
     *              <SignatureMethod Algorithm="http://www.w3.org/2000/09/xmldsig#rsa-sha1"/>
     *              <Reference URI="#_0">
     *                          <Transforms>
     *                              <Transform Algorithm="http://www.w3.org/2001/10/xml-exc-c14n#"/>
     *                          </Transforms>
     *                          <DigestMethod Algorithm="http://www.w3.org/2000/09/xmldsig#sha1"/>
     *                          <DigestValue>TrxNllK6n3YWNFF3xSfwVt+E/SU=</DigestValue>
     *               </Reference>
     *               <Reference URI="#_1">
     *                          <Transforms>
     *                              <Transform Algorithm="http://www.w3.org/2001/10/xml-exc-c14n#"/></Transforms>
     *                              <DigestMethod Algorithm="http://www.w3.org/2000/09/xmldsig#sha1"/>
     *                              <DigestValue>3PmxFbDh3nLPSs6EXGYiEyS8aqE=</DigestValue>
     *               </Reference>
     *          </SignedInfo>
     *          <SignatureValue>LixDZD6...</SignatureValue>
     *          <KeyInfo>
     *              <o:SecurityTokenReference>
     *                  <o:Reference URI="#uuid-5a2be53c-cd9d-42cf-8075-a02f029bf72e-1"/>
     *              </o:SecurityTokenReference>
     *           </KeyInfo>
     *       </Signature>
     *      </o:Security>
     * 
     * 
     * Client set up
     * 
     * Set the client certificate
     * 
     * scf.Credentials.ClientCertificate.Certificate =MyCertificates.GetClientCertificate();
       
     * Service set up
     * 
     *   // Client certificate x509certificate validator

            host.Credentials.ClientCertificate.Authentication.
                  CertificateValidationMode = X509CertificateValidationMode.Custom;

            host.Credentials.ClientCertificate.Authentication.
             CustomCertificateValidator = new CustomX509CertificateValidator();
     * 
     * **/
    public class CertificateOverTransportBinding : CustomBinding
    {
        public CertificateOverTransportBinding()
        {
            //add  security
            var securityElement =
                SecurityBindingElement.CreateCertificateOverTransportBindingElement();


            Elements.Add(securityElement);

            // Message Encoding
            var textEncoding = new TextMessageEncodingBindingElement();
            textEncoding.MessageVersion = MessageVersion.Soap12WSAddressing10;
            Elements.Add(textEncoding);

            // Transport
            Elements.Add(new HttpsTransportBindingElement());

        }
    }
}
