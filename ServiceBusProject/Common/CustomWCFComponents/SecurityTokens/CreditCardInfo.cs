

using System;

namespace CustomWCFComponents.SecurityTokens
{
    //https://msdn.microsoft.com/en-us/library/ms731872%28v=vs.110%29.aspx
    public class CreditCardInfo
    {
        string cardNumber;
        string cardIssuer;
        DateTime expirationDate;

        public CreditCardInfo(string cardNumber, string cardIssuer, DateTime expirationDate)
        {
            this.cardNumber = cardNumber;
            this.cardIssuer = cardIssuer;
            this.expirationDate = expirationDate;
        }

        public string CardNumber
        {
            get { return this.cardNumber; }
        }

        public string CardIssuer
        {
            get { return this.cardIssuer; }
        }

        public DateTime ExpirationDate
        {
            get { return this.expirationDate; }
        }
    }
}
