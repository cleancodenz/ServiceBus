

using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel.Security.Tokens;

namespace CustomWCFComponents.SecurityTokens
{
    public class CreditCardTokenParameters : SecurityTokenParameters
    {
        public CreditCardTokenParameters()
        {
        }

        protected CreditCardTokenParameters(CreditCardTokenParameters other)
            : base(other)
        {
        }

        /// <summary>
        /// Copy all internal fields defined in your class, if any. 
        /// This example does not define any additional fields.
        /// </summary>
        /// <returns></returns>
        protected override SecurityTokenParameters CloneCore()
        {
            return new CreditCardTokenParameters(this);
        }

        /// <summary>
        /// Implement the InitializeSecurityTokenRequirement(SecurityTokenRequirement) method. 
        /// This method is called by WCF to convert the security token parameters class instance into an instance of the SecurityTokenRequirement class. 
        /// The result is used by security token providers to create the appropriate security token instance
        /// </summary>
        /// <param name="requirement"></param>
        protected override void InitializeSecurityTokenRequirement(SecurityTokenRequirement requirement)
        {
            requirement.TokenType = CreditCardTokenConstants.CreditCardTokenType;
            return;
        }

        // A credit card token has no cryptography, no windows identity, and supports only client authentication.
        protected override bool HasAsymmetricKey
        {
            get { return false; }
        }

        /// <summary>
        /// Implement the SupportsClientAuthentication read-only property. 
        /// This property returns true if the security token type represented by this class can be used to authenticate a client to a service. 
        /// In this example, the credit card security token can be used to authenticate a client to a service.
        /// </summary>
        protected override bool SupportsClientAuthentication
        {
            get { return true; }
        }

        /// <summary>
        /// Implement the SupportsClientWindowsIdentity read-only property. 
        /// This property returns true if the security token type represented by this class can be mapped to a Windows account. 
        /// If so, the authentication result is represented by a WindowsIdentity class instance. 
        /// In this example, the token cannot be mapped to a Windows account.
        /// 
        /// </summary>
        protected override bool SupportsClientWindowsIdentity
        {
            get { return false; }
        }

        /// <summary>
        /// Implement the SupportsServerAuthentication read-only property. 
        /// This property returns true if the security token type represented by this class can be used to authenticate a service to a client. 
        /// In this example, the credit card security token cannot be used to authenticate a service to a client
        /// 
        /// </summary>
        protected override bool SupportsServerAuthentication
        {
            get { return false; }
        }

        /// <summary>
        /// Implement the CreateKeyIdentifierClause(SecurityToken, SecurityTokenReferenceStyle) method. 
        /// This method is called by WCF security framework when it requires a reference to the security token instance represented by this security token parameters class.
        /// Both the actual security token instance and SecurityTokenReferenceStyle that specifies the type of the reference that is being requested are passed to this method as arguments. In this example, only internal references are supported by the credit card security token. The SecurityToken class has functionality to create internal references; therefore, the implementation does not require additional code.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="referenceStyle"></param>
        /// <returns></returns>
        protected override SecurityKeyIdentifierClause CreateKeyIdentifierClause(SecurityToken token, SecurityTokenReferenceStyle referenceStyle)
        {
            if (referenceStyle == SecurityTokenReferenceStyle.Internal)
            {
                return token.CreateKeyIdentifierClause<LocalIdKeyIdentifierClause>();
            }
            else
            {
                throw new NotSupportedException("External references are not supported for credit card tokens");
            }
        }
    }

}
