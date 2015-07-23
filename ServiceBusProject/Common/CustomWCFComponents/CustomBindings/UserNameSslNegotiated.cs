

using System.ServiceModel.Channels;

namespace CustomWCFComponents.CustomBindings
{
    /**
     * With this authentication mode the client is authenticates using a Username Token which appears at the SOAP layer as a signed supporting token. The service is authenticated using an X.509 certificate
     * this is using WS-Trust requesting token first, then using the token to encrypt the message
     * Looks like the TokenType is alreay SecurityContextToken which is also used by WS-SecureConversation
     * 
     * Request of token
     * 
     * <s:Body>
     *      <t:RequestSecurityToken Context="uuid-a84f91c7-7501-471b-83d7-cdbb3b1ffb10-1" xmlns:t="http://schemas.xmlsoap.org/ws/2005/02/trust">
     *          <t:TokenType>http://schemas.xmlsoap.org/ws/2005/02/sc/sct</t:TokenType>
     *          <t:RequestType>http://schemas.xmlsoap.org/ws/2005/02/trust/Issue</t:RequestType>
     *          <t:KeySize>256</t:KeySize>
     *          <t:BinaryExchange ValueType=" http://schemas.xmlsoap.org/ws/2005/02/trust/tlsnego" EncodingType="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary">
     *              FgM...A=</t:BinaryExchange>
     *      </t:RequestSecurityToken>
     *</s:Body>
     * 
     * Response of Token request
     * 
     * <s:Body>
     *      <t:RequestSecurityTokenResponse Context="uuid-a84f91c7-7501-471b-83d7-cdbb3b1ffb10-1" xmlns:t="http://schemas.xmlsoap.org/ws/2005/02/trust" xmlns:u="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd">
     *          <t:BinaryExchange ValueType=" http://schemas.xmlsoap.org/ws/2005/02/trust/tlsnego" EncodingType="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary">
     *           FgM...DgAAAA==</t:BinaryExchange>
     *       </t:RequestSecurityTokenResponse>
     * </s:Body>
     * 
     * Request of Security Token Response
     * 
     * <s:Body>
     *      <t:RequestSecurityTokenResponse Context="uuid-a84f91c7-7501-471b-83d7-cdbb3b1ffb10-1" xmlns:t="http://schemas.xmlsoap.org/ws/2005/02/trust" xmlns:u="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd">
     *          <t:BinaryExchange ValueType=" http://schemas.xmlsoap.org/ws/2005/02/trust/tlsnego" EncodingType="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary">
     *              FgM...yxkuhdti97k=</t:BinaryExchange>
     *      </t:RequestSecurityTokenResponse>
     * </s:Body>
     * 
     * Response of Request of Security Token Response
     * 
     * <s:Body>
     *      <t:RequestSecurityTokenResponseCollection xmlns:t="http://schemas.xmlsoap.org/ws/2005/02/trust">
     *          <t:RequestSecurityTokenResponse Context="uuid-a84f91c7-7501-471b-83d7-cdbb3b1ffb10-1" xmlns:u="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd">
     *              <t:TokenType>http://schemas.xmlsoap.org/ws/2005/02/sc/sct</t:TokenType>
     *              <t:RequestedSecurityToken>
     *                  <c:SecurityContextToken u:Id="uuid-62370337-8af2-4e0f-831c-dcbd9e37db01-1" xmlns:c="http://schemas.xmlsoap.org/ws/2005/02/sc">
     *                      <c:Identifier>urn:uuid:d65ced26-babb-49e9-a0f6-e46de6ab0fc6</c:Identifier>
     *                      <dnse:Cookie xmlns:dnse="http://schemas.microsoft.com/ws/2006/05/security">
     *                          AQAAANCMnd8BF...MeBbPnDUJ3SHXujQ==</dnse:Cookie>
     *                   </c:SecurityContextToken>
     *              </t:RequestedSecurityToken>
     *              <t:RequestedAttachedReference>
     *                  <o:SecurityTokenReference xmlns:o="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd">
     *                      <o:Reference URI="#uuid-62370337-8af2-4e0f-831c-dcbd9e37db01-1"></o:Reference>
     *                   </o:SecurityTokenReference>
     *               </t:RequestedAttachedReference>
     *               <t:RequestedUnattachedReference>
     *                  <o:SecurityTokenReference xmlns:o="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd">
     *                      <o:Reference URI="urn:uuid:d65ced26-babb-49e9-a0f6-e46de6ab0fc6" ValueType="http://schemas.xmlsoap.org/ws/2005/02/sc/sct"></o:Reference>
     *                  </o:SecurityTokenReference>
     *               </t:RequestedUnattachedReference>
     *               <t:RequestedProofToken>
     *                  <e:EncryptedKey xmlns:e="http://www.w3.org/2001/04/xmlenc#">
     *                      <e:EncryptionMethod Algorithm="http://schemas.xmlsoap.org/2005/02/trust/tlsnego#TLS_Wrap"></e:EncryptionMethod>
     *                      <e:CipherData>
     *                          <e:CipherValue>FwMBAECiu...YQtTm2Hj9V</e:CipherValue>
     *                      </e:CipherData>
     *                   </e:EncryptedKey>
     *                </t:RequestedProofToken>
     *                <t:Lifetime>
     *                  <u:Created>2015-03-15T21:26:58.638Z</u:Created>
     *                  <u:Expires>2015-03-16T07:26:58.638Z</u:Expires>
     *                </t:Lifetime>
     *                <t:KeySize>256</t:KeySize>
     *                <t:BinaryExchange ValueType=" http://schemas.xmlsoap.org/ws/2005/02/trust/tlsnego" EncodingType="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary">
     *                  FAMBAAEBFgMBAD...tNMFpRY2Wg=</t:BinaryExchange>
     *           </t:RequestSecurityTokenResponse>
     *           <t:RequestSecurityTokenResponse Context="uuid-a84f91c7-7501-471b-83d7-cdbb3b1ffb10-1" xmlns:u="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd">
     *           <t:Authenticator>
     *              <t:CombinedHash>YLKN2LCEN7+2iJaOUtLw5xc8cVs/UFBXJMGo8iNxb2A=</t:CombinedHash>
     *           </t:Authenticator>
     *          </t:RequestSecurityTokenResponse>
     *      </t:RequestSecurityTokenResponseCollection>
     *  </s:Body>
     * 
     * Service request with security header
     * 
     * <o:Security s:mustUnderstand="1" xmlns:o="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd">
     *      <u:Timestamp u:Id="uuid-a84f91c7-7501-471b-83d7-cdbb3b1ffb10-2">
     *          <u:Created>2015-03-15T21:26:58.779Z</u:Created>
     *          <u:Expires>2015-03-15T21:31:58.779Z</u:Expires>
     *      </u:Timestamp>
     *      <c:SecurityContextToken u:Id="uuid-62370337-8af2-4e0f-831c-dcbd9e37db01-1" xmlns:c="http://schemas.xmlsoap.org/ws/2005/02/sc">
     *          <c:Identifier>urn:uuid:d65ced26-babb-49e9-a0f6-e46de6ab0fc6</c:Identifier>
     *          <dnse:Cookie xmlns:dnse="http://schemas.microsoft.com/ws/2006/05/security">
     *              AQAAANCMnd8B...lEz36spFGMeBbPnDUJ3SHXujQ==</dnse:Cookie>
     *      </c:SecurityContextToken>
     *      <c:DerivedKeyToken u:Id="_0" xmlns:c="http://schemas.xmlsoap.org/ws/2005/02/sc">
     *          <o:SecurityTokenReference>
     *              <o:Reference URI="#uuid-62370337-8af2-4e0f-831c-dcbd9e37db01-1"/>
     *          </o:SecurityTokenReference>
     *          <c:Offset>0</c:Offset>
     *          <c:Length>24</c:Length>
     *          <c:Nonce>vorfFu/388DpUA/z7qbjow==</c:Nonce>
     *       </c:DerivedKeyToken>
     *       <c:DerivedKeyToken u:Id="_1" xmlns:c="http://schemas.xmlsoap.org/ws/2005/02/sc">
     *          <o:SecurityTokenReference>
     *              <o:Reference URI="#uuid-62370337-8af2-4e0f-831c-dcbd9e37db01-1"/>
     *          </o:SecurityTokenReference>
     *          <c:Nonce>PysHiIiliFa0xdxszrENxA==</c:Nonce>
     *       </c:DerivedKeyToken>
     *       <e:ReferenceList xmlns:e="http://www.w3.org/2001/04/xmlenc#">
     *          <e:DataReference URI="#_3"/>
     *          <e:DataReference URI="#_8"/>
     *          <e:DataReference URI="#_9"/>
     *       </e:ReferenceList>
     *       <e:EncryptedData Id="_9" Type="http://www.w3.org/2001/04/xmlenc#Element" xmlns:e="http://www.w3.org/2001/04/xmlenc#">
     *          <e:EncryptionMethod Algorithm="http://www.w3.org/2001/04/xmlenc#aes256-cbc"/>
     *          <KeyInfo xmlns="http://www.w3.org/2000/09/xmldsig#">
     *              <o:SecurityTokenReference>
     *                  <o:Reference URI="#_1"/>
     *              </o:SecurityTokenReference>
     *          </KeyInfo>
     *          <e:CipherData>
     *              <e:CipherValue>kUkC8Bd19I9tvW...j+wXnqzm0UANuyvSvDYs=</e:CipherValue>
     *          </e:CipherData>
     *       </e:EncryptedData>
     *       <e:EncryptedData Id="_8" Type="http://www.w3.org/2001/04/xmlenc#Element" xmlns:e="http://www.w3.org/2001/04/xmlenc#">
     *          <e:EncryptionMethod Algorithm="http://www.w3.org/2001/04/xmlenc#aes256-cbc"/>
     *          <KeyInfo xmlns="http://www.w3.org/2000/09/xmldsig#">
     *              <o:SecurityTokenReference>
     *                  <o:Reference URI="#_1"/>
     *              </o:SecurityTokenReference>
     *          </KeyInfo>
     *          <e:CipherData>
     *              <e:CipherValue>AMeJS5gA2.........VTA1mDMabTerCtEHOLANrx</e:CipherValue>
     *          </e:CipherData>
     *      </e:EncryptedData>
     *</o:Security>
     * 
     * 
     * REsponse of service
     *      * 
     * <s:Envelope xmlns:s="http://www.w3.org/2003/05/soap-envelope" xmlns:a="http://www.w3.org/2005/08/addressing" xmlns:u="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd">
     *      <s:Header>
     *          <a:Action s:mustUnderstand="1" u:Id="_4">http://tempuri.org/ICalculator/MultiplyResponse</a:Action>
     *          <a:RelatesTo u:Id="_5">urn:uuid:5637af78-c4f0-4fc5-a7c8-88c506e7749a</a:RelatesTo>
     *          <o:Security s:mustUnderstand="1" xmlns:o="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd">
     *              <u:Timestamp u:Id="uuid-62370337-8af2-4e0f-831c-dcbd9e37db01-2">
     *                  <u:Created>2015-03-15T21:27:01.478Z</u:Created>
     *                  <u:Expires>2015-03-15T21:32:01.478Z</u:Expires>
     *              </u:Timestamp>
     *              <c:DerivedKeyToken u:Id="_0" xmlns:c="http://schemas.xmlsoap.org/ws/2005/02/sc">
     *                  <o:SecurityTokenReference>
     *                      <o:Reference URI="urn:uuid:d65ced26-babb-49e9-a0f6-e46de6ab0fc6" ValueType="http://schemas.xmlsoap.org/ws/2005/02/sc/sct"/>
     *                  </o:SecurityTokenReference>
     *                  <c:Offset>0</c:Offset>
     *                  <c:Length>24</c:Length>
     *                  <c:Nonce>Zbu0HpDVljtJx4Lgo/vyww==</c:Nonce>
     *               </c:DerivedKeyToken>
     *               <c:DerivedKeyToken u:Id="_1" xmlns:c="http://schemas.xmlsoap.org/ws/2005/02/sc">
     *                  <o:SecurityTokenReference>
     *                      <o:Reference URI="urn:uuid:d65ced26-babb-49e9-a0f6-e46de6ab0fc6" ValueType="http://schemas.xmlsoap.org/ws/2005/02/sc/sct"/>
     *                  </o:SecurityTokenReference>
     *                  <c:Nonce>PUeavvqCnBQiJt3Ai24ayQ==</c:Nonce>
     *               </c:DerivedKeyToken>
     *               <e:ReferenceList xmlns:e="http://www.w3.org/2001/04/xmlenc#">
     *                  <e:DataReference URI="#_3"/>
     *                  <e:DataReference URI="#_6"/>
     *               </e:ReferenceList>
     *               <e:EncryptedData Id="_6" Type="http://www.w3.org/2001/04/xmlenc#Element" xmlns:e="http://www.w3.org/2001/04/xmlenc#">
     *               <e:EncryptionMethod Algorithm="http://www.w3.org/2001/04/xmlenc#aes256-cbc"/>
     *               <KeyInfo xmlns="http://www.w3.org/2000/09/xmldsig#">
     *                  <o:SecurityTokenReference>
     *                      <o:Reference URI="#_1"/>
     *                  </o:SecurityTokenReference>
     *               </KeyInfo>
     *               <e:CipherData>
     *                  <e:CipherValue>iDDY52AZiSXWnpHK9yN.....n/B1obO1M1v4IWY</e:CipherValue>
     *               </e:CipherData>
     *          </e:EncryptedData>
     *    </o:Security>
     *</s:Header>
     *<s:Body u:Id="_2">
     *  <e:EncryptedData Id="_3" Type="http://www.w3.org/2001/04/xmlenc#Content" xmlns:e="http://www.w3.org/2001/04/xmlenc#">
     *      <e:EncryptionMethod Algorithm="http://www.w3.org/2001/04/xmlenc#aes256-cbc"/>
     *      <KeyInfo xmlns="http://www.w3.org/2000/09/xmldsig#">
     *          <o:SecurityTokenReference xmlns:o="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd">
     *              <o:Reference URI="#_1"/>
     *          </o:SecurityTokenReference>
     *      </KeyInfo>
     *      <e:CipherData>
     *          <e:CipherValue>lyRPouj4MWUgQfCiWRu...zQKk=</e:CipherValue>
     *      </e:CipherData>
     *  </e:EncryptedData>
     *</s:Body>
     *</s:Envelope>
     * 
     * 
     * 
     * Client
     * 
     *   // Need a customized X509CertificateValidator
              
                var clientCredentials = scf.Credentials;

                clientCredentials.ServiceCertificate.Authentication.
                    CertificateValidationMode = X509CertificateValidationMode.Custom;
                clientCredentials.ServiceCertificate.Authentication.
                    CustomCertificateValidator = new CustomX509CertificateValidator();
            

                // user name
                scf.Credentials.UserName.UserName = "testUser";
                scf.Credentials.UserName.Password = "p@ssword";
    
     * 
     * 
     * Service
     * 
     *  //Load the service certificate
            var certificate = GetCustomedServerCertificate();
            // var certificate = GetServerCertificate();

            host.Credentials.ServiceCertificate.Certificate = certificate;


            //Change user name authentication 
            host.Credentials.UserNameAuthentication.
                UserNamePasswordValidationMode = UserNamePasswordValidationMode.MembershipProvider;

            host.Credentials.UserNameAuthentication.MembershipProvider
                 = new MyMemberShipProvider();

            //set the authorization of user
            var serviceAuthroization = host.Authorization;
            serviceAuthroization.PrincipalPermissionMode = PrincipalPermissionMode.UseAspNetRoles;
            serviceAuthroization.RoleProvider = new MyRoleProvider();
     * 
     * 
     * **/
    public class UserNameSslNegotiated: CustomBinding
    {
       public UserNameSslNegotiated()
       {
           //add  security
           var securityElement =
               SecurityBindingElement.CreateUserNameForSslBindingElement();


           Elements.Add(securityElement);

           // Message Encoding
           var textEncoding = new TextMessageEncodingBindingElement();
           textEncoding.MessageVersion = MessageVersion.Soap12WSAddressing10;
           Elements.Add(textEncoding);

           // Transport
           Elements.Add(new HttpTransportBindingElement());
       }
    }
}
