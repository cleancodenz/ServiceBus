﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Security.Principal;
using System.Web.Security;
using CustomWCFComponents.OldClaimsConstant;

namespace CustomWCFComponents.AuthorizationPolicy
{
    /**
     * This is old way of claims, new claim is in System.Security using WIF
     * **/
    public class MyClaimsAuthorizationPolicy : IAuthorizationPolicy
    {

        string id = Guid.NewGuid().ToString();

        /**
        This method receives the claim sets evaluated so far by other authorization policies.
        For example, it may include a claim set for each token passed in the request message, 
         * thus contain a WindowsClaimSet or UserNameClaimSet or x509 cliams set and so on.
         * 
         * Responsible for inspecting claims based on the credentials provided, 
         * mapping those claims to normalized claims, 
         * and constructing a security principal for the request thread. 
         * 
         * The method should return false if this authorization policy was not able to complete its authorization. 
         * 
         * If false, the service model will invoke other authorization policies and then call this one once more, passing the updated claim sets. 
         * This gives the authorization policy another chance to authorize calls.
         * 
        **/
        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
           
            object obj;
            if (!evaluationContext.Properties.TryGetValue("Identities", out obj))
                return false;

            IList<IIdentity> identities = obj as IList<IIdentity>;
            if (obj == null || identities.Count <= 0)
                return false;

            IIdentity identity = identities[0];

            //This is claims conversion
            ClaimSet claims = MapClaims(identity);

            if (claims == null)
                return false;


            GenericPrincipal newPrincipal = new GenericPrincipal(identity, null);

            evaluationContext.Properties["Principal"] = newPrincipal;

            evaluationContext.AddClaimSet(this, claims);

            return true;
        }

        // returns a ClaimSet describing the issuer associated with the authorization policy. If claims are generated by this authorization policy, it must provide an issuer when generating the claim set for the authorization context.

        public ClaimSet Issuer
        {
            get { return ClaimSet.System; }
        }

        //returns a unique identifier for the authorization policy instance. This can be a unique GUID

        public string Id
        {
            get { return id; }
        }

        private ClaimSet MapClaims(IIdentity identity)
        {
            List<Claim> listClaims = new List<Claim>();

            // check authentication type first
            switch (identity.AuthenticationType)
            {
                case "X509":
                    // this certificate, name is subject+thumprint
                    // the identity only has name and one claim which is name claim of subject of certificate
                    if (identity.Name == "CN=Johnson.Test.Client; 5CBC2377DA40CBF644277AC7010266DE989B0737")
                    {
                        listClaims.Add(new Claim(
                            OperationClaimTypes.Read,
                            ResourceClaimTypes.Customers,
                            Rights.PossessProperty));

                        listClaims.Add(new Claim(
                            OperationClaimTypes.Read,
                             ResourceClaimTypes.Customers,
                             Rights.PossessProperty));
                    }
                    break;
                case "MembershipProviderValidator": // user name
                    // Already assigned role for testUser
                    if (identity.Name == "testUser")
                    {
                        listClaims.Add(new Claim(
                            OperationClaimTypes.Read,
                             ResourceClaimTypes.Customers,
                             Rights.PossessProperty));

                        listClaims.Add(new Claim(
                            OperationClaimTypes.Read,
                             ResourceClaimTypes.Customers,
                             Rights.PossessProperty));

                    }

                    break;
                default:
                    break;

            }


            // where issuer is System which has a few system based claims

            return new DefaultClaimSet(ClaimSet.System, listClaims);

            // or create a issuer claims, which issuer it self
          //  string IssuerName = IssuerClaimTypes.Issuer;

          //  Claim c = Claim.CreateNameClaim(IssuerName);
          //  Claim[] claims = new Claim[1];
          //  claims[0] = c;
          //  DefaultClaimSet issuerClaimSet = new DefaultClaimSet(claims);

           // return new DefaultClaimSet(issuerClaimSet, listClaims);
        }
    }
}
