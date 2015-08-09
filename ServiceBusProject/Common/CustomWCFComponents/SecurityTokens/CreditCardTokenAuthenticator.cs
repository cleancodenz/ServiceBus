

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;

namespace CustomWCFComponents.SecurityTokens
{
    /// <summary>
    /// The security token authenticator validates the content of the security token when it is extracted from the message. 
    /// The following example overrides the ValidateTokenCore method.
    /// 
    /// </summary>
    public class CreditCardTokenAuthenticator : SecurityTokenAuthenticator
    {
     
        private List<CreditCardInfo> allValidCreditCards;
        public CreditCardTokenAuthenticator(List<CreditCardInfo> AllValidCreditCards)
        {
            allValidCreditCards = AllValidCreditCards;
        }

        protected override bool CanValidateTokenCore(SecurityToken token)
        {
            return (token is CreditCardToken);
        }

        protected override ReadOnlyCollection<IAuthorizationPolicy> ValidateTokenCore(SecurityToken token)
        {
            CreditCardToken creditCardToken = token as CreditCardToken;

            if (creditCardToken.CardInfo.ExpirationDate < DateTime.UtcNow)
            {
                throw new SecurityTokenValidationException("The credit card has expired");
            }
            if (!IsCardNumberAndExpirationValid(creditCardToken.CardInfo))
            {
                throw new SecurityTokenValidationException("Unknown or invalid credit card");
            }

            // The credit card token has only 1 claim: the card number. The issuer for the claim is the
            // credit card issuer.
            DefaultClaimSet cardIssuerClaimSet = new DefaultClaimSet(new Claim(ClaimTypes.Name, creditCardToken.CardInfo.CardIssuer, Rights.PossessProperty));
            DefaultClaimSet cardClaimSet = new DefaultClaimSet(cardIssuerClaimSet, new Claim(CreditCardTokenConstants.CreditCardNumberClaim, creditCardToken.CardInfo.CardNumber, Rights.PossessProperty));
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>(1);
            policies.Add(new CreditCardTokenAuthorizationPolicy(cardClaimSet));
            return policies.AsReadOnly();
        }

        // This helper method checks whether a given credit card entry is present in the user database.
        private bool IsCardNumberAndExpirationValid(CreditCardInfo cardInfo)
        {

            /** original version is using a creditcardfile to store all valid credit cards
            try
            {
                using (StreamReader myStreamReader = new StreamReader(this.creditCardsFile))
                {
                    string line = "";
                    while ((line = myStreamReader.ReadLine()) != null)
                    {
                        string[] splitEntry = line.Split('#');
                        if (splitEntry[0] == cardInfo.CardNumber)
                        {
                            string expirationDateString = splitEntry[1].Trim();
                            DateTime expirationDateOnFile = DateTime.Parse(expirationDateString, System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.AdjustToUniversal);
                            if (cardInfo.ExpirationDate == expirationDateOnFile)
                            {
                                string issuer = splitEntry[2];
                                return issuer.Equals(cardInfo.CardIssuer, StringComparison.InvariantCultureIgnoreCase);
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception("BookStoreService: Error while retrieving credit card information from User DB " + e.ToString());
            }
             * */
         // new version just using a list of credicards
            return allValidCreditCards.Exists(c=>c.CardNumber == cardInfo.CardNumber);
        }
    }
}
