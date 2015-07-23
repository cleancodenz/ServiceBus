

using System;
using System.IdentityModel.Claims;
using System.ServiceModel;
using CustomWCFComponents.OldClaimsConstant;

namespace CustomWCFComponents.ServiceAuthorizationManager
{
    public class MyServiceAuthorizationManager : System.ServiceModel.ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            string action = operationContext.RequestContext.RequestMessage.Headers.Action;

            // accessing operationContext.ServiceSecurityContext.AuthorizationContext.ClaimSets to trigger
            // IAuthorizationPolicy.Evaluate claims transformation
            foreach (ClaimSet cs in operationContext.ServiceSecurityContext.AuthorizationContext.ClaimSets)
            {
                if (cs.Issuer == ClaimSet.System)
                {
                    foreach (Claim c in cs.FindClaims(OperationClaimTypes.Read, Rights.PossessProperty))
                    {
                        if (c.Resource.ToString()== ResourceClaimTypes.Customers)
                            return true;
                    }
                }
            }
            return false;
        }
    }
}
