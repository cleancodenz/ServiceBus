

using System.ServiceModel.Channels;


namespace CustomWCFComponents.CustomBindings
{
    /**
     * Need https
     * 
     * WS-Security request header
     * <o:Security s:mustUnderstand="1" xmlns:o="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd">
     *      <u:Timestamp u:Id="_0">
     *          <u:Created>2015-03-09T01:57:26.783Z</u:Created>
     *          <u:Expires>2015-03-09T02:02:26.783Z</u:Expires>
     *      </u:Timestamp>
     *      <o:UsernameToken u:Id="uuid-b2b5aad2-078b-4f9d-860d-630c662e878d-1">
     *          <o:Username>testUser</o:Username>
     *          <o:Password>p@ssword</o:Password>
     *     </o:UsernameToken>
     * </o:Security>
     * 
     * 
     * Response
     * <o:Security s:mustUnderstand="1" xmlns:o="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd">
     *  <u:Timestamp u:Id="_0"> ... </u:Timestamp>
     * </o:Security>
     * 
     * Client
     * 
     *     scf.Credentials.UserName.UserName = "testUser";
           scf.Credentials.UserName.Password = "p@ssword";
     * 
     * Service
     * 
     *      //Change user name authentication 
         host.Credentials.UserNameAuthentication.
             UserNamePasswordValidationMode = UserNamePasswordValidationMode.MembershipProvider ;

         host.Credentials.UserNameAuthentication.MembershipProvider
              = new MyMemberShipProvider();
 
         //set the authorization of user or claims transformation
         var serviceAuthroization = host.Authorization;
         serviceAuthroization.PrincipalPermissionMode = PrincipalPermissionMode.UseAspNetRoles;
         serviceAuthroization.RoleProvider = new MyRoleProvider();
     * 
     * **/
    public class UserNameOverTransportBinding : CustomBinding
    {
       
        public UserNameOverTransportBinding()
        {
            //add  security
            var securityElement =
                SecurityBindingElement.CreateUserNameOverTransportBindingElement();

        
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
