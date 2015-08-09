

using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.ServiceModel.Description;

namespace CustomWCFComponents.SecurityTokens
{
    public class CreditCardServiceCredentials : ServiceCredentials
    {
       // string creditCardFile;
        List<CreditCardInfo> allValidCreditCards;
        public CreditCardServiceCredentials(List<CreditCardInfo> AllValidCreditCards)
            : base()
        {
            if (AllValidCreditCards == null)
            {
                throw new ArgumentNullException("creditCardFile");
            }

            this.allValidCreditCards = AllValidCreditCards;
        }

        /**
        public string CreditCardDataFile
        {
            get { return this.creditCardFile; }
        }
        **/

        public List<CreditCardInfo> ValidCreditCards
        {
            get { return allValidCreditCards; }
        }

        protected override ServiceCredentials CloneCore()
        {
            return new CreditCardServiceCredentials(allValidCreditCards);
        }

        public override SecurityTokenManager CreateSecurityTokenManager()
        {
            return new CreditCardServiceCredentialsSecurityTokenManager(this);
        }
    }
}
