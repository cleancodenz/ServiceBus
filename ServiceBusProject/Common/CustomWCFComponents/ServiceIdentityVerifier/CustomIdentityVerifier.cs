using System.IdentityModel.Claims;
using System.ServiceModel;
using System.ServiceModel.Security;

namespace CustomWCFComponents.ServiceIdentityVerifier
{
    // Used by client to customized service identitychecking

    public class CustomIdentityVerifier : IdentityVerifier
    {
        public override bool CheckAccess(EndpointIdentity identity, System.IdentityModel.Policy.AuthorizationContext authContext)
        {
            bool returnvalue = false;

            foreach (ClaimSet claimset in authContext.ClaimSets)
            {
                foreach (Claim claim in claimset)
                {
                   // if (claim.ClaimType == "http://schemas.microsoft.com/ws/2005/05/identity/claims/x500distinguishedname")
                   // {
                   //     X500DistinguishedName name = (X500DistinguishedName)claim.Resource;
                        //if (name.Name.Contains(((OrgEndpointIdentity)identity).OrganizationClaim))
                       // {
                           // Console.WriteLine("Claim Type: {0}", claim.ClaimType);
                          //  Console.WriteLine("Right: {0}", claim.Right);
                          //  Console.WriteLine("Resource: {0}", claim.Resource);
                          //  Console.WriteLine();
                            returnvalue = true;
                            break;
                    //}
                    //}
                }

            }
            return returnvalue;
        }

        public override bool TryGetIdentity(EndpointAddress reference, out EndpointIdentity identity)
        {
            return CreateDefault().TryGetIdentity(reference, out identity);
        }

    
    }
}
