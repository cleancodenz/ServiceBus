

using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel;
using System.ServiceModel.Security.Tokens;

namespace CustomWCFComponents.SecurityTokens
{
    /// <summary>
    /// The security token manager creates the appropriate token provider, security authenticator, and token serializer instances. 
    /// The primary methods of the class use a SecurityTokenRequirement to create the appropriate provider and client or service credentials.
    /// 
    /// </summary>
    public class CreditCardClientCredentialsSecurityTokenManager : ClientCredentialsSecurityTokenManager
    {
        CreditCardClientCredentials creditCardClientCredentials;

        public CreditCardClientCredentialsSecurityTokenManager(CreditCardClientCredentials creditCardClientCredentials)
            : base(creditCardClientCredentials)
        {
            this.creditCardClientCredentials = creditCardClientCredentials;
        }

        public override SecurityTokenProvider CreateSecurityTokenProvider(SecurityTokenRequirement tokenRequirement)
        {
            if (tokenRequirement.TokenType == CreditCardTokenConstants.CreditCardTokenType)
            {
                // Handle this token for Custom.
                return new CreditCardTokenProvider(this.creditCardClientCredentials.CreditCardInfo);
            }
            else if (tokenRequirement is InitiatorServiceModelSecurityTokenRequirement)
            {
                // Return server certificate.
                if (tokenRequirement.TokenType == SecurityTokenTypes.X509Certificate)
                {
                    return new X509SecurityTokenProvider(creditCardClientCredentials.ServiceCertificate.DefaultCertificate);
                }
            }
            return base.CreateSecurityTokenProvider(tokenRequirement);
        }

        public override SecurityTokenSerializer CreateSecurityTokenSerializer(SecurityTokenVersion version)
        {
            return new CreditCardSecurityTokenSerializer(version);
        }

    }
}
