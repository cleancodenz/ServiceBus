

using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;

namespace CustomWCFComponents.SecurityTokens
{
    /// <summary>
    /// The security token provider creates, modifies (if necessary), and returns an instance of the token.
    /// The following example overrides the GetTokenCore method to return an instance of the CreditCardToken
    /// 
    /// </summary>
    public class CreditCardTokenProvider : SecurityTokenProvider
    {
        CreditCardInfo creditCardInfo;

        public CreditCardTokenProvider(CreditCardInfo creditCardInfo)
            : base()
        {
            if (creditCardInfo == null)
            {
                throw new ArgumentNullException("creditCardInfo");
            }
            this.creditCardInfo = creditCardInfo;
        }

        protected override SecurityToken GetTokenCore(TimeSpan timeout)
        {
            SecurityToken result = new CreditCardToken(this.creditCardInfo);
            return result;
        }
    }
}
