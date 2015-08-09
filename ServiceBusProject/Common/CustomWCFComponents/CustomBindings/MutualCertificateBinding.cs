

using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security.Tokens;
using MyBehaviors;


namespace CustomWCFComponents.CustomBindings
{
    /**
     * No need of https
     * 
     * This is WSSecurity1.1  not WS-Trust
     * 
     * Request security header
     * 
     * <o:Security s:mustUnderstand="1" xmlns:o="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd">
     *      <u:Timestamp u:Id="uuid-64ac3763-0b39-474d-a470-b53c89d94c99-2">
     *          <u:Created>2015-03-09T03:51:46.582Z</u:Created>
     *          <u:Expires>2015-03-09T03:56:46.582Z</u:Expires>
     *      </u:Timestamp>
     *      <e:EncryptedKey Id="uuid-64ac3763-0b39-474d-a470-b53c89d94c99-1" xmlns:e="http://www.w3.org/2001/04/xmlenc#">
     *          <e:EncryptionMethod Algorithm="http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p">
     *              <DigestMethod Algorithm="http://www.w3.org/2000/09/xmldsig#sha1" xmlns="http://www.w3.org/2000/09/xmldsig#"/>
     *          </e:EncryptionMethod>
     *          <KeyInfo xmlns="http://www.w3.org/2000/09/xmldsig#">
     *              <o:SecurityTokenReference>
     *                  <o:KeyIdentifier ValueType="http://docs.oasis-open.org/wss/oasis-wss-soap-message-security-1.1#ThumbprintSHA1">VVSZ....0=</o:KeyIdentifier>
     *              </o:SecurityTokenReference>
     *           </KeyInfo>
     *           <e:CipherData>
     *              <e:CipherValue>Wv9ny...g1Nsf/Li5zcYuDIw==</e:CipherValue>
     *            </e:CipherData>
     *       </e:EncryptedKey>
     *       <c:DerivedKeyToken u:Id="_0" xmlns:c="http://schemas.xmlsoap.org/ws/2005/02/sc">
     *          <o:SecurityTokenReference>
     *              <o:Reference URI="#uuid-64ac3763-0b39-474d-a470-b53c89d94c99-1"/></o:SecurityTokenReference>
     *              <c:Offset>0</c:Offset>
     *              <c:Length>24</c:Length>
     *              <c:Nonce>WYbO8uGHCNzUj/R1cgBwiw==</c:Nonce>
     *        </c:DerivedKeyToken>
     *        <c:DerivedKeyToken u:Id="_2" xmlns:c="http://schemas.xmlsoap.org/ws/2005/02/sc">
     *          <o:SecurityTokenReference>
     *              <o:Reference URI="#uuid-64ac3763-0b39-474d-a470-b53c89d94c99-1"/></o:SecurityTokenReference>
     *              <c:Nonce>JEV3D5m1cIQ0CfFZ7x787Q==</c:Nonce></c:DerivedKeyToken>
     *              <e:ReferenceList xmlns:e="http://www.w3.org/2001/04/xmlenc#">
     *                  <e:DataReference URI="#_4"/><e:DataReference URI="#_9"/>
     *                  <e:DataReference URI="#_10"/></e:ReferenceList>
     *                  <o:BinarySecurityToken u:Id="uuid-f2d9f5d4-596d-4fe4-acfd-cad9d77460ee-1" ValueType="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3">MIIDE...hNd</o:BinarySecurityToken>
     *                  <e:EncryptedData Id="_9" Type="http://www.w3.org/2001/04/xmlenc#Element" xmlns:e="http://www.w3.org/2001/04/xmlenc#">
     *                      <e:EncryptionMethod Algorithm="http://www.w3.org/2001/04/xmlenc#aes256-cbc"/>
     *                          <KeyInfo xmlns="http://www.w3.org/2000/09/xmldsig#">
     *                              <o:SecurityTokenReference><o:Reference URI="#_2"/></o:SecurityTokenReference>
     *                          </KeyInfo>
     *                          <e:CipherData>
     *                              <e:CipherValue>XG6ua....Sjap+scVADxOe</e:CipherValue>
     *                          </e:CipherData>
     *                       </e:EncryptedData>
     *                       <e:EncryptedData Id="_10" Type="http://www.w3.org/2001/04/xmlenc#Element" xmlns:e="http://www.w3.org/2001/04/xmlenc#">
     *                          <e:EncryptionMethod Algorithm="http://www.w3.org/2001/04/xmlenc#aes256-cbc"/>
     *                          <KeyInfo xmlns="http://www.w3.org/2000/09/xmldsig#">
     *                              <o:SecurityTokenReference>
     *                                  <o:Reference URI="#_2"/>
     *                              </o:SecurityTokenReference>
     *                           </KeyInfo>
     *                           <e:CipherData>
     *                              <e:CipherValue>tafBj3...b72CckJKA==</e:CipherValue>
     *                            </e:CipherData>
     *                     </e:EncryptedData>
     *       </o:Security>
     * 
     * 
     * Client
     * 
     *         scf.Credentials.ServiceCertificate.DefaultCertificate = 
                    MyCertificates.GetServiceCertificate();

                // Specify a certificate to use for authenticating the client.
                clientCredentials.ClientCertificate.Certificate =
                    MyCertificates.GetClientCertificate();
                
     * 
     * Service
     * 
     * 
            host.Credentials.ServiceCertificate.Certificate = certificate;


            // Need custom x509certificate validator to validate client certificate
            host.Credentials.ClientCertificate.Authentication.
                  CertificateValidationMode = X509CertificateValidationMode.Custom;

            host.Credentials.ClientCertificate.Authentication.
             CustomCertificateValidator = new CustomX509CertificateValidator();

 
     * **/
    public class MutualCertificateBinding : CustomBinding
    {

        public MutualCertificateBinding()
        {
            //add  security
            //var securityElement =
           //     SecurityBindingElement.CreateMutualCertificateBindingElement();

            var securityElement = new SymmetricSecurityBindingElement();

            var x509TokenParameters = new X509SecurityTokenParameters();
            // how to find certificate
            // this will be used by securitymanager to find the certificate when create x509security tokens
            //x509TokenParameters.X509ReferenceStyle = X509KeyIdentifierClauseType.Thumbprint;

            //The token is never included in messages but is referenced. The token must be known to the recipient out of band
            x509TokenParameters.InclusionMode = SecurityTokenInclusionMode.Never;
            securityElement.ProtectionTokenParameters = x509TokenParameters;

            var supportingx509TokenParameters = new X509SecurityTokenParameters();
            supportingx509TokenParameters.InclusionMode = SecurityTokenInclusionMode.AlwaysToRecipient;

            // adding the support to endorsing 
            securityElement.EndpointSupportingTokenParameters.
               Endorsing.Add(supportingx509TokenParameters);
            
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
