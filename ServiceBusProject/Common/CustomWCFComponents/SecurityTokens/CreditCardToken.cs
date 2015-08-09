

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;

namespace CustomWCFComponents.SecurityTokens
{
    public class CreditCardToken : SecurityToken
    {
        CreditCardInfo cardInfo;
        DateTime effectiveTime = DateTime.UtcNow;
        string id;
        ReadOnlyCollection<SecurityKey> securityKeys;

        public CreditCardToken(CreditCardInfo cardInfo) : this(cardInfo, Guid.NewGuid().ToString()) { }

        public CreditCardToken(CreditCardInfo cardInfo, string id)
        {
            if (cardInfo == null)
            {
                throw new ArgumentNullException("cardInfo");
            }
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            this.cardInfo = cardInfo;
            this.id = id;
            // The credit card token is not capable of any cryptography.
            this.securityKeys = new ReadOnlyCollection<SecurityKey>(new List<SecurityKey>());
        }

        public CreditCardInfo CardInfo
        {
            get { return this.cardInfo; }
        }

        /// <summary>
        /// Implement the SecurityKeys property. 
        /// This property returns a collection of security keys that the security token instance represents. 
        /// Such keys can be used by WCF to sign or encrypt parts of the SOAP message. 
        /// In this example, the credit card security token cannot contain any security keys; 
        /// therefore, the implementation always returns an empty collection.
        /// </summary>
        public override ReadOnlyCollection<SecurityKey> SecurityKeys
        {
            get { return this.securityKeys; }
        }
        /// <summary>
        /// Override the ValidFrom and ValidTo properties. 
        /// These properties are used by WCF to determine the validity of the security token instance. 
        /// In this example, the credit card security token has only an expiration date, 
        /// so the ValidFrom property returns a DateTime that represents the date and time of the instance creation.
        /// </summary>
        public override DateTime ValidFrom
        {
            get { return this.effectiveTime; }
        }

        public override DateTime ValidTo
        {
            get { return this.cardInfo.ExpirationDate; }
        }

        /// <summary>
        /// Override the Id property. 
        /// This property is used to get the local identifier of the security token that is used to point to the security token XML representation 
        /// from other elements inside the SOAP message. 
        /// In this example, a token identifier can be either passed to it as a constructor parameter 
        /// or a new random one is generated every time a security token instance is created.
        /// 
        /// </summary>
        public override string Id
        {
            get { return this.id; }
        }
    }
}
