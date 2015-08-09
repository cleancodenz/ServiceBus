

using System.ServiceModel.Channels;

namespace CustomWCFComponents.CustomBindings
{
    /**
     *This is secure conversation built based on user name negotiation 
     *
     * SecurityTokenRequest
     * 
     * <s:Body>
     *      <t:RequestSecurityToken Context="uuid-9fd5ef90-7c73-4c4c-852c-69bddd817da6-1" xmlns:t="http://schemas.xmlsoap.org/ws/2005/02/trust">
     *          <t:TokenType>http://schemas.xmlsoap.org/ws/2005/02/sc/sct</t:TokenType>
     *          <t:RequestType>http://schemas.xmlsoap.org/ws/2005/02/trust/Issue</t:RequestType>
     *          <t:KeySize>256</t:KeySize>
     *          <t:BinaryExchange ValueType=" http://schemas.xmlsoap.org/ws/2005/02/trust/tlsnego" EncodingType="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary">
     *              FgMBAFoBAABWAwFVBghS....BAAXABgACwACAQA=</t:BinaryExchange>
     *       </t:RequestSecurityToken>
     * </s:Body>
     * 
     * Response of Security Token Request
     * 
     * <s:Body>
     *      <t:RequestSecurityTokenResponse Context="uuid-9fd5ef90-7c73-4c4c-852c-69bddd817da6-1" xmlns:t="http://schemas.xmlsoap.org/ws/2005/02/trust" xmlns:u="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd">
     *          <t:BinaryExchange ValueType=" http://schemas.xmlsoap.org/ws/2005/02/trust/tlsnego" EncodingType="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary">
     *              FgMBA3cCAABNAwFVBghXkWYR...1dO853MBSvPDgAAAA==</t:BinaryExchange>
     *      </t:RequestSecurityTokenResponse>
     *</s:Body>
     *
     * Request of Security Token Response
     * 
     * <s:Body>
     *      <t:RequestSecurityTokenResponse Context="uuid-9fd5ef90-7c73-4c4c-852c-69bddd817da6-1" xmlns:t="http://schemas.xmlsoap.org/ws/2005/02/trust" xmlns:u="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd">
     *          <t:BinaryExchange ValueType=" http://schemas.xmlsoap.org/ws/2005/02/trust/tlsnego" EncodingType="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary">
     *              FgMBAQYQAAECAQCHh/ms98....XPlDazQicQ9Wuh84=</t:BinaryExchange>
     *      </t:RequestSecurityTokenResponse>
     * </s:Body>
     * 
     * Response of Request of Security Token Response
     * 
     * <s:Body>
     *      <t:RequestSecurityTokenResponseCollection xmlns:t="http://schemas.xmlsoap.org/ws/2005/02/trust">
     *          <t:RequestSecurityTokenResponse Context="uuid-9fd5ef90-7c73-4c4c-852c-69bddd817da6-1" xmlns:u="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd">
     *              <t:TokenType>http://schemas.xmlsoap.org/ws/2005/02/sc/sct</t:TokenType>
     *              <t:RequestedSecurityToken>
     *                  <c:SecurityContextToken u:Id="uuid-6fc9e406-b164-4afd-a20d-437677b0b60f-1" xmlns:c="http://schemas.xmlsoap.org/ws/2005/02/sc">
     *                      <c:Identifier>urn:uuid:01c4796c-46f4-40b3-a4b9-c383de2673b1</c:Identifier>
     *                      <dnse:Cookie xmlns:dnse="http://schemas.microsoft.com/ws/2006/05/security">
     *                          AQAAANCMnd8BFdER...8m2TkFSx3Me4jE8Qwg==</dnse:Cookie>
     *                  </c:SecurityContextToken>
     *           </t:RequestedSecurityToken>
     *           <t:RequestedAttachedReference>
     *              <o:SecurityTokenReference xmlns:o="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd">
     *                  <o:Reference URI="#uuid-6fc9e406-b164-4afd-a20d-437677b0b60f-1"></o:Reference>
     *              </o:SecurityTokenReference>
     *           </t:RequestedAttachedReference>
     *           <t:RequestedUnattachedReference>
     *              <o:SecurityTokenReference xmlns:o="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd">
     *                  <o:Reference URI="urn:uuid:01c4796c-46f4-40b3-a4b9-c383de2673b1" ValueType="http://schemas.xmlsoap.org/ws/2005/02/sc/sct"></o:Reference>
     *              </o:SecurityTokenReference>
     *           </t:RequestedUnattachedReference>
     *           <t:RequestedProofToken>
     *              <e:EncryptedKey xmlns:e="http://www.w3.org/2001/04/xmlenc#">
     *                  <e:EncryptionMethod Algorithm="http://schemas.xmlsoap.org/2005/02/trust/tlsnego#TLS_Wrap"></e:EncryptionMethod>
     *                  <e:CipherData>
     *                      <e:CipherValue>FwMBAECKd........40+TmzvlRyDboKu0s</e:CipherValue>
     *                  </e:CipherData>
     *              </e:EncryptedKey>
     *            </t:RequestedProofToken>
     *            <t:Lifetime>
     *                  <u:Created>2015-03-15T22:31:51.798Z</u:Created>
     *                  <u:Expires>2015-03-16T08:31:51.798Z</u:Expires>
     *            </t:Lifetime>
     *            <t:KeySize>256</t:KeySize>
     *            <t:BinaryExchange ValueType=" http://schemas.xmlsoap.org/ws/2005/02/trust/tlsnego" EncodingType="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary">
     *              FAMBAAEBFgMBADCGsRw...FlW01Kf1OgCA=</t:BinaryExchange>
     *            </t:RequestSecurityTokenResponse>
     *            <t:RequestSecurityTokenResponse Context="uuid-9fd5ef90-7c73-4c4c-852c-69bddd817da6-1" xmlns:u="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd">
     *              <t:Authenticator>
     *                  <t:CombinedHash>LDPri8inIFpPJzCI4ua6YNy4+OPSFTEKCLQzEpWQ8Xc=</t:CombinedHash>
     *              </t:Authenticator>
     *             </t:RequestSecurityTokenResponse>
     *      </t:RequestSecurityTokenResponseCollection>
     *  </s:Body>
     * 
     * 
     * Request of challenge of derived keys
     * 
     * 
     * <o:Security s:mustUnderstand="1" xmlns:o="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd">
     *      <u:Timestamp u:Id="uuid-9fd5ef90-7c73-4c4c-852c-69bddd817da6-3">
     *          <u:Created>2015-03-15T22:31:51.954Z</u:Created>
     *          <u:Expires>2015-03-15T22:36:51.954Z</u:Expires>
     *       </u:Timestamp>
     *       <c:SecurityContextToken u:Id="uuid-6fc9e406-b164-4afd-a20d-437677b0b60f-1" xmlns:c="http://schemas.xmlsoap.org/ws/2005/02/sc">
     *          <c:Identifier>urn:uuid:01c4796c-46f4-40b3-a4b9-c383de2673b1</c:Identifier>
     *          <dnse:Cookie xmlns:dnse="http://schemas.microsoft.com/ws/2006/05/security">
     *              AQAAANCMnd8BFdER...Me4jE8Qwg==</dnse:Cookie>
     *        </c:SecurityContextToken>
     *        <c:DerivedKeyToken u:Id="_0" xmlns:c="http://schemas.xmlsoap.org/ws/2005/02/sc">
     *          <o:SecurityTokenReference><o:Reference URI="#uuid-6fc9e406-b164-4afd-a20d-437677b0b60f-1"/>
     *          </o:SecurityTokenReference>
     *          <c:Offset>0</c:Offset>
     *          <c:Length>24</c:Length>
     *          <c:Nonce>aodds8qECZCZJepNDkSK8A==</c:Nonce>
     *        </c:DerivedKeyToken>
     *        <c:DerivedKeyToken u:Id="_1" xmlns:c="http://schemas.xmlsoap.org/ws/2005/02/sc">
     *          <o:SecurityTokenReference><o:Reference URI="#uuid-6fc9e406-b164-4afd-a20d-437677b0b60f-1"/>
     *          </o:SecurityTokenReference>
     *          <c:Nonce>6tpp6+HKll/VCYNDrqo5Wg==</c:Nonce>
     *        </c:DerivedKeyToken>
     *        <e:ReferenceList xmlns:e="http://www.w3.org/2001/04/xmlenc#">
     *          <e:DataReference URI="#_3"/>
     *          <e:DataReference URI="#_8"/>
     *          <e:DataReference URI="#_9"/>
     *        </e:ReferenceList>
     *        <e:EncryptedData Id="_9" Type="http://www.w3.org/2001/04/xmlenc#Element" xmlns:e="http://www.w3.org/2001/04/xmlenc#">
     *          <e:EncryptionMethod Algorithm="http://www.w3.org/2001/04/xmlenc#aes256-cbc"/>
     *          <KeyInfo xmlns="http://www.w3.org/2000/09/xmldsig#">
     *              <o:SecurityTokenReference>
     *                  <o:Reference URI="#_1"/></o:SecurityTokenReference>
     *           </KeyInfo>
     *           <e:CipherData>
     *              <e:CipherValue>Efqq0P1...SyvRXZLLa+KH2sM=</e:CipherValue>
     *           </e:CipherData>
     *         </e:EncryptedData>
     *         <e:EncryptedData Id="_8" Type="http://www.w3.org/2001/04/xmlenc#Element" xmlns:e="http://www.w3.org/2001/04/xmlenc#">
     *              <e:EncryptionMethod Algorithm="http://www.w3.org/2001/04/xmlenc#aes256-cbc"/>
     *              <KeyInfo xmlns="http://www.w3.org/2000/09/xmldsig#">
     *                  <o:SecurityTokenReference><o:Reference URI="#_1"/></o:SecurityTokenReference>
     *              </KeyInfo>
     *              <e:CipherData>
     *                  <e:CipherValue>22lUaleaTUhT+d4g6n8pTc7TGYsc...gn/P40kt9fED5hEvpRfSXWN7</e:CipherValue>
     *              </e:CipherData>
     *          </e:EncryptedData>
     *    </o:Security>
     * 
     * Response of Request of challenge of derived keys
     * 
     * ..
     * 
     * Second of Request of challenge of derived keys
     * ...
     * 
     * Response of  Second of Request of challenge of derived keys
     * ...
     * 
     * Request of service
     * 
     * <o:Security s:mustUnderstand="1" xmlns:o="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd">
     *      <u:Timestamp u:Id="uuid-9fd5ef90-7c73-4c4c-852c-69bddd817da6-7">
     *          <u:Created>2015-03-15T22:31:55.680Z</u:Created>
     *          <u:Expires>2015-03-15T22:36:55.680Z</u:Expires>
     *      </u:Timestamp>
     *      <c:SecurityContextToken u:Id="uuid-6fc9e406-b164-4afd-a20d-437677b0b60f-2" xmlns:c="http://schemas.xmlsoap.org/ws/2005/02/sc">
     *          <c:Identifier>urn:uuid:81a9cc88-8c1e-4df9-862e-cefbffe76566</c:Identifier>
     *      </c:SecurityContextToken>
     *      <c:DerivedKeyToken u:Id="uuid-9fd5ef90-7c73-4c4c-852c-69bddd817da6-4" xmlns:c="http://schemas.xmlsoap.org/ws/2005/02/sc">
     *          <o:SecurityTokenReference>
     *              <o:Reference URI="#uuid-6fc9e406-b164-4afd-a20d-437677b0b60f-2"/>
     *          </o:SecurityTokenReference>
     *          <c:Offset>0</c:Offset>
     *          <c:Length>24</c:Length>
     *          <c:Nonce>bQIpBVNcEan+UzqCQhm8OA==</c:Nonce>
     *       </c:DerivedKeyToken>
     *       <Signature xmlns="http://www.w3.org/2000/09/xmldsig#">
     *          <SignedInfo>
     *              <CanonicalizationMethod Algorithm="http://www.w3.org/2001/10/xml-exc-c14n#"/>
     *              <SignatureMethod Algorithm="http://www.w3.org/2000/09/xmldsig#hmac-sha1"/>
     *                  <Reference URI="#_0">
     *                      <Transforms>
     *                          <Transform Algorithm="http://www.w3.org/2001/10/xml-exc-c14n#"/>
     *                      </Transforms>
     *                      <DigestMethod Algorithm="http://www.w3.org/2000/09/xmldsig#sha1"/>
     *                      <DigestValue>GZs5kqXqXTL/BYsfMKPT3LQ9m/8=</DigestValue>
     *                   </Reference>
     *                   <Reference URI="#_1">
     *                      <Transforms>
     *                          <Transform Algorithm="http://www.w3.org/2001/10/xml-exc-c14n#"/>
     *                      </Transforms>
     *                      <DigestMethod Algorithm="http://www.w3.org/2000/09/xmldsig#sha1"/>
     *                      <DigestValue>GA0icwQsAzfUOq7Gy1Cg8CTTk6o=</DigestValue>
     *                    </Reference>
     *                    <Reference URI="#_2">
     *                          <Transforms>
     *                              <Transform Algorithm="http://www.w3.org/2001/10/xml-exc-c14n#"/>
     *                          </Transforms>
     *                          <DigestMethod Algorithm="http://www.w3.org/2000/09/xmldsig#sha1"/>
     *                          <DigestValue>2pqPt72HA0o9Fsol5PWQ4bfZp+U=</DigestValue>
     *                     </Reference>
     *                     <Reference URI="#_3">
     *                          <Transforms>
     *                              <Transform Algorithm="http://www.w3.org/2001/10/xml-exc-c14n#"/>
     *                          </Transforms>
     *                          <DigestMethod Algorithm="http://www.w3.org/2000/09/xmldsig#sha1"/>
     *                          <DigestValue>o3ibE52LCPwycD7dwAsKtJa+WMw=</DigestValue>
     *                       </Reference>
     *                       <Reference URI="#_4">
     *                          <Transforms>
     *                              <Transform Algorithm="http://www.w3.org/2001/10/xml-exc-c14n#"/>
     *                              </Transforms>
     *                              <DigestMethod Algorithm="http://www.w3.org/2000/09/xmldsig#sha1"/>
     *                              <DigestValue>wismR+PeWWqQaL7yBu9fnlIGk8s=</DigestValue>
     *                        </Reference>
     *                        <Reference URI="#uuid-9fd5ef90-7c73-4c4c-852c-69bddd817da6-7">
     *                          <Transforms>
     *                              <Transform Algorithm="http://www.w3.org/2001/10/xml-exc-c14n#"/>
     *                          </Transforms>
     *                          <DigestMethod Algorithm="http://www.w3.org/2000/09/xmldsig#sha1"/>
     *                          <DigestValue>54kP0XfRWdC5vqZ1cg3GgWXqKPI=</DigestValue>
     *                        </Reference>
     *                     </SignedInfo>
     *                     <SignatureValue>ZziLWCzQhpmJkJkqUB9SdsGQebo=</SignatureValue>
     *                     <KeyInfo>
     *                          <o:SecurityTokenReference>
     *                              <o:Reference URI="#uuid-9fd5ef90-7c73-4c4c-852c-69bddd817da6-4"/>
     *                           </o:SecurityTokenReference>
     *                     </KeyInfo>
     *          </Signature>
     *   </o:Security>
     * 
     * Response of Request of service
     * ...
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
     * **/

    public class UserNameSecureConversationBinding : CustomBinding
    {
        public UserNameSecureConversationBinding()
        {

            //add  security
            var oneShotSecurity =
                SecurityBindingElement.CreateUserNameForSslBindingElement();

            var securityElement =
                SecurityBindingElement.CreateSecureConversationBindingElement(oneShotSecurity);

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
